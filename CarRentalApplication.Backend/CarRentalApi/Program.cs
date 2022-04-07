using CarRentalApi.BusinessLayer.MapperProfiles;
using CarRentalApi.BusinessLayer.Services;
using CarRentalApi.BusinessLayer.Validators;
using CarRentalApi.DataAccessLayer;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CarRentalApi", Version = "v1" });
});
builder.Services.AddAutoMapper(typeof(PersonMapperProfile).Assembly);
builder.Services.AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<SavePersonRequestValidator>();
});

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
    options.UseSqlServer(connectionString, dbOptions =>
    {
        dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(2), null);
    });
});
builder.Services.AddScoped<IDataContext>(service => service.GetRequiredService<DataContext>());
builder.Services.AddScoped<IReadOnlyDataContext>(service => service.GetRequiredService<DataContext>());

builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IVehiclesService, VehiclesService>();
builder.Services.AddScoped<IReservationsService, ReservationsService>();

var app = builder.Build();
app.UseProblemDetails();
app.UseSwagger();
app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "CarRentalApi v1"));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();