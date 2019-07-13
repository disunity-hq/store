using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Disunity.Store.Shared.Data;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Disunity.Store.Entities {

    public class Target {

        public int ID { get; set; }

        public int? LatestId { get; set; }

        public TargetVersion Latest { get; set; }

        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        [Required] [MaxLength(128)] public string Slug { get; set; }

        [InverseProperty("Target")] public List<TargetVersion> Versions { get; set; }


        [OnAfterCreate(typeof(TargetVersion))]
        public static void UpdateLatestVersion(TargetVersion newVersion, ApplicationDbContext context,
                                               IServiceProvider services) {
            var target = context.Targets.FirstOrDefault(t => t.ID == newVersion.TargetId);

            if (target == null) {
                return;
            }

            target.Latest = newVersion;
            context.SaveChanges();

        }

        public class TargetConfiguration : IEntityTypeConfiguration<Target> {

            public void Configure(EntityTypeBuilder<Target> builder) {
                builder.HasOne(t => t.Latest)
                       .WithOne(v => v.Target);

                builder.HasMany(t => t.Versions);

                builder.HasAlternateKey(t => t.DisplayName);
            }

        }

    }

}