using CarRentalApi.DataAccessLayer.Configurations.Common;
using CarRentalApi.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApi.DataAccessLayer.Configurations;

internal class ReservationConfiguration : BaseEntityConfiguration<Reservation>
{
    public override void Configure(EntityTypeBuilder<Reservation> builder)
    {
        base.Configure(builder);

        builder.ToTable("Reservations");

        builder.HasOne(r => r.Person)
            .WithMany(p => p.Reservations)
            .HasForeignKey(r => r.IdPerson)
            .IsRequired();

        builder.HasOne(r => r.Vehicle)
            .WithMany(v => v.Reservations)
            .HasForeignKey(r => r.IdVehicle)
            .IsRequired();

        builder.Property(r => r.Start)
            .IsRequired();

        builder.Property(r => r.Finish)
            .IsRequired();
    }
}