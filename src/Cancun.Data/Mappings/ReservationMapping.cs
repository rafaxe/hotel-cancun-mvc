using Cancun.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cancun.Data.Mappings
{
    public class ReservationMapping : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CheckIn)
                .IsRequired()
                .HasColumnType("timestamp without time zone");

            builder.Property(p => p.CheckOut)
                .IsRequired()
                .HasColumnType("timestamp without time zone");

            builder.Property(p => p.Days)
                .IsRequired()
                .HasColumnType("integer");

            builder.Property(p => p.PriceTotal)
                .IsRequired()
                .HasColumnType("numeric");

            builder.HasOne(p => p.Suite).WithMany(c =>
             c.Reservations).HasForeignKey("SuiteId");

            builder.ToTable("Reservations");
        }
    }
}