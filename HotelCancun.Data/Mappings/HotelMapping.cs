using HotelCancun.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelCancun.Data.Mappings
{
    public class HotelMapping : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Hotel : Address
            builder.HasOne(f => f.Address)
                .WithOne(e => e.Hotel);

            // 1 : N => Hotel : Suites
            builder.HasMany(f => f.Suites)
                .WithOne(p => p.Hotel)
                .HasForeignKey(p => p.HotelId);

            builder.ToTable("Hotels");
        }
    }
}