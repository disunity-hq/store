using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class UnityVersion {

        public int ID { get; set; }

        [Required] public string Version { get; set; }

        public class UnityVersionConfiguration : IEntityTypeConfiguration<UnityVersion> {

            public void Configure(EntityTypeBuilder<UnityVersion> builder) {
                builder.HasIndex(v => v.Version).IsUnique();
            }

        }

    }

}