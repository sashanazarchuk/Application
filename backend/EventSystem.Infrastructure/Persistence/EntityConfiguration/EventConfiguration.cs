using EventSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.EntityConfiguration
{
    internal class EventConfiguration:IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            builder.Property(e => e.Location)
                .HasMaxLength(255);

            builder.Property(e => e.Date)
                .IsRequired();

            builder.HasOne(e => e.Admin)
                .WithMany(u => u.AdminEvents)
                .HasForeignKey(e => e.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}