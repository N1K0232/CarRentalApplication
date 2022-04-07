using CarRentalApi.DataAccessLayer.Configurations.Common;
using CarRentalApi.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApi.DataAccessLayer.Configurations;

internal class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.ToTable("People");

        builder.Property(p => p.FirstName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .IsRequired();

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.EmailAddress)
            .HasMaxLength(50)
            .IsRequired();
    }
}