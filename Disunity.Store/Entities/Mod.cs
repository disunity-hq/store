using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Data.Hooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class Mod : TrackableModel {

        public Mod() {
            IsActive = true;
            IsDeprecated = false;
            IsPinned = false;
        }

        public int Id { get; set; }

        public int? OwnerId { get; set; }
        public Org Owner { get; set; }

        [Required] [MaxLength(128)] public string Name { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeprecated { get; set; }
        public bool? IsPinned { get; set; }

        public int? LatestId { get; set; }
        public ModVersion Latest { get; set; }

        [InverseProperty("Mod")] public List<ModVersion> Versions { get; set; }

        [OnBeforeCreate(typeof(ModVersion))]
        public static void OnBeforeCreateModVersion(ModVersion entity, ApplicationDbContext context) {
            // TODO Figure out if this works
            // TODO Should we be checking to see if it's the most recent version? (only necesary if we don't enforce always increasing)
            entity.Mod.Latest = entity;
        }

        public class ModConfiguration : IEntityTypeConfiguration<Mod> {

            public void Configure(EntityTypeBuilder<Mod> builder) {
                builder.Property(m => m.IsActive).HasDefaultValue(true);
                builder.Property(m => m.IsDeprecated).HasDefaultValue(false);
                builder.Property(m => m.IsPinned).HasDefaultValue(false);

                builder.HasAlternateKey(m => m.Name);

                builder
                    .HasOne(m => m.Owner)
                    .WithMany(o => o.Mods)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasOne(m => m.Latest);

                builder
                    .HasMany(m => m.Versions)
                    .WithOne(v => v.Mod);
            }

        }

    }

}