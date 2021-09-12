using Cancun.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cancun.Data.Mappings
{
    public class SuiteMapping : IEntityTypeConfiguration<Suite>
    {
        public void Configure(EntityTypeBuilder<Suite> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Image)
                .IsRequired()
                .HasColumnType("varchar(100)");

            // 1 : N => Suite : Reservations
            builder.HasMany(f => f.Reservations)
                .WithOne(p => p.Suite)
                .HasForeignKey(p => p.SuiteId);

            builder.ToTable("Suites");
        }
    }
}