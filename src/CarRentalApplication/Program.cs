using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using CarRentalApplication.BusinessLayer;
using CarRentalApplication.BusinessLayer.Settings;
using CarRentalApplication.DataAccessLayer;
using CarRentalApplication.ExceptionHandlers;
using CarRentalApplication.Extensions;
using CarRentalApplication.StorageProviders.Extensions;
using CarRentalApplication.Swagger;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OperationResults.AspNetCore.Http;
using TinyHelpers.AspNetCore.Extensions;
using TinyHelpers.AspNetCore.Swagger;
using TinyHelpers.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();
Configure(app, app.Environment, app.Services);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
{
    var appSettings = services.ConfigureAndGet<AppSettings>(configuration, nameof(AppSettings));
    var swaggerSettings = services.ConfigureAndGet<SwaggerSettings>(configuration, nameof(SwaggerSettings));

    services.AddHttpContextAccessor();
    services.AddRazorPages();

    services.AddExceptionHandler<DefaultExceptionHandler>();
    services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = context =>
        {
            var statusCode = context.ProblemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);
            context.ProblemDetails.Type ??= $"https://httpstatuses.io/{statusCode}";
            context.ProblemDetails.Title ??= ReasonPhrases.GetReasonPhrase(statusCode);
            context.ProblemDetails.Instance ??= context.HttpContext.Request.Path;
            context.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        };
    });

    services.AddWebOptimizer(minifyCss: true, minifyJavaScript: environment.IsProduction());
    services.AddRequestLocalization(appSettings.SupportedCultures);

    services.AddOperationResult(options =>
    {
        options.ErrorResponseFormat = ErrorResponseFormat.List;
    });

    services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.SerializerOptions.Converters.Add(new UtcDateTimeConverter());
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    services.AddFluentValidationAutoValidation(options =>
    {
        options.DisableDataAnnotationsValidation = true;
    });

    if (swaggerSettings.Enabled)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "CarRentalApi", Version = "v1" });
            options.AddDefaultResponse();
            options.AddAcceptLanguageHeader();

            options.MapType<DateTime>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Example = new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            });

            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString(DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd"))
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.UseAllOfToExtendReferenceSchemas();
            options.IncludeXmlComments(xmlPath);
        })
        .AddFluentValidationRulesToSwagger(options =>
        {
            options.SetNotNullableIfMinLengthGreaterThenZero = true;
        });
    }

    services.AddSqlServer<DataContext>(configuration.GetConnectionString("SqlConnection"));
    services.AddScoped<IDataContext>(services => services.GetRequiredService<DataContext>());

    if (environment.IsDevelopment())
    {
        services.AddFileSystemStorage(options =>
        {
            options.StorageFolder = appSettings.StorageFolder;
        });
    }
    else
    {
        services.AddAzureStorage(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("AzureStorageConnection");
            options.ContainerName = appSettings.ContainerName;
        });
    }
}

void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider services)
{
    var appSettings = services.GetRequiredService<IOptions<AppSettings>>().Value;
    var swaggerSettings = services.GetRequiredService<IOptions<SwaggerSettings>>().Value;

    environment.ApplicationName = appSettings.ApplicationName;

    app.UseHttpsRedirection();
    app.UseRequestLocalization();

    app.UseRouting();
    app.UseWebOptimizer();

    app.UseWhen(context => context.IsWebRequest(), builder =>
    {
        if (!environment.IsDevelopment())
        {
            builder.UseExceptionHandler("/Errors/500");
            builder.UseHsts();
        }

        builder.UseStatusCodePagesWithReExecute("/Errors/{0}");
    });

    app.UseWhen(context => context.IsApiRequest(), builder =>
    {
        builder.UseExceptionHandler();
        builder.UseStatusCodePages();
    });

    app.UseDefaultFiles();
    app.UseStaticFiles();

    if (swaggerSettings.Enabled)
    {
        app.UseMiddleware<SwaggerAuthenticationMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CarRentalApi v1");
            options.InjectStylesheet("/css/swagger.css");
        });
    }

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
    });
}