using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class Target {

        public int ID { get; set; }

        public int? LatestId { get; set; }
        public TargetVersion Latest { get; set; }

        [Required] [MaxLength(128)] public string Name { get; set; }

        [InverseProperty("Target")] public List<TargetVersion> Versions { get; set; }

        public class TargetConfiguration : IEntityTypeConfiguration<Target> {

            public void Configure(EntityTypeBuilder<Target> builder) {
                builder.HasOne(t => t.Latest)
                       .WithOne(v => v.Target);

                builder.HasMany(t => t.Versions);

                builder.HasAlternateKey(t => t.Name);
            }

        }

    }

}