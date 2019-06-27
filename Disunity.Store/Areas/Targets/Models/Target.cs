using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Targets.Models
{
    public class Target
    {
        public int ID { get; set; }

        public int? LatestId { get; set; }
        public TargetVersion Latest { get; set; }

        [InverseProperty("Target")] public List<TargetVersion> Versions { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Target>()
                .HasOne(t => t.Latest)
                .WithOne(v => v.Target);

            builder.Entity<Target>()
                .HasMany(t => t.Versions);
        }
    }
}