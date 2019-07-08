using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class DisunityVersion {

        public int ID { get; set; }

        [DataType(DataType.Url)] public string URL { get; set; }

        [Required] public string Version { get; set; }

        [InverseProperty("Version")] public DisunityVersionCompatibility CompatibileUnityVersion { get; set; }

        public class DisunityVersionConfiguration : IEntityTypeConfiguration<DisunityVersion> {

            public void Configure(EntityTypeBuilder<DisunityVersion> builder) {
                builder.HasIndex(v => v.Version).IsUnique();
            }

        }

    }

}