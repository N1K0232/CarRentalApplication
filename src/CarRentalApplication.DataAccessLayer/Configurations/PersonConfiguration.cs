using CarRentalApplication.DataAccessLayer.Configurations.Common;
using CarRentalApplication.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApplication.DataAccessLayer.Configurations;

public class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(256).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(256).IsRequired();

        builder.Property(p => p.BirthDate).IsRequired();
        builder.Property(p => p.Gender).HasMaxLength(10).IsRequired();

        builder.Property(p => p.IdentityCardNumber).HasMaxLength(100).IsRequired();
        builder.Property(p => p.FiscalCode).HasMaxLength(50).IsRequired(false);

        builder.Property(p => p.City).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Province).HasMaxLength(20).IsRequired();

        builder.Property(p => p.CellphoneNumber).HasMaxLength(20).IsUnicode(false).IsRequired();
        builder.Property(p => p.EmailAddress).HasMaxLength(256).IsUnicode(false).IsRequired();

        builder.HasIndex(p => p.CellphoneNumber)
            .HasDatabaseName("IX_CellphoneNumber")
            .IsUnique();

        builder.HasIndex(p => p.EmailAddress)
            .HasDatabaseName("IX_EmailAddress")
            .IsUnique();

        builder.ToTable("People");
        base.Configure(builder);
    }
}