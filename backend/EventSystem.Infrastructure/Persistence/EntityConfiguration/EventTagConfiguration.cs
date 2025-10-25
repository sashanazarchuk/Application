using EventSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.EntityConfiguration
{
    internal class EventTagConfiguration : IEntityTypeConfiguration<EventTag>
    {

        public void Configure(EntityTypeBuilder<EventTag> builder)
        {
            builder.HasKey(et => new { et.EventId, et.TagId });  

            builder.HasOne(et => et.Event)
                   .WithMany(e => e.EventTags)   
                   .HasForeignKey(et => et.EventId);

            builder.HasOne(et => et.Tag)
                   .WithMany(t => t.EventTags)   
                   .HasForeignKey(et => et.TagId);
        }
    }
}
