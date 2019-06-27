using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disunity.Store.Areas.Mods.Models
{
    public class Mod : TrackableModel
    {
        public Mod()
        {
            IsActive = true;
            IsDeprecated = false;
            IsPinned = false;
        }

        public int Id { get; set; }

        public int? OwnerId { get; set; }
        public Org Owner { get; set; }

        [Required] [MaxLength(128)] public string Name { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeprecated { get; set; }
        public bool IsPinned { get; set; }

        public int? LatestId { get; set; }
        public ModVersion Latest { get; set; }

        [InverseProperty("Mod")] public List<ModVersion> Versions { get; set; }

        public class ModConfiguration : IEntityTypeConfiguration<Mod>
        {
            public void Configure(EntityTypeBuilder<Mod> builder)
            {
                builder.Property(m => m.IsActive).HasDefaultValue(true);
                builder.Property(m => m.IsDeprecated).HasDefaultValue(false);
                builder.Property(m => m.IsPinned).HasDefaultValue(false);

                builder
                    .HasOne(m => m.Owner)
                    .WithMany(o => o.Mods)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasOne(m => m.Latest)
                    .WithOne(v => v.Mod);

                builder
                    .HasMany(m => m.Versions);
            }
        }
    }
}