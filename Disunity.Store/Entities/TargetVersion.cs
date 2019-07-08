using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class TargetVersion {

        public int ID { get; set; }

        public int TargetId { get; set; }
        public Target Target { get; set; }

        [Required] [MaxLength(128)] public string Name { get; set; }

        [Required] [MaxLength(16)] public string VersionNumber { get; set; }

        [InverseProperty("Version")] public TargetVersionCompatibility DisunityCompatibility { get; set; }

        [Required]
        [MaxLength(1024)]
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }

        [Required] [MaxLength(256)] public string Description { get; set; }

        [Required]
        [MaxLength(1024)]
        [DataType(DataType.ImageUrl)]
        public string IconUrl { get; set; }

        public class TargetVersionConfiguration : IEntityTypeConfiguration<TargetVersion> {

            public void Configure(EntityTypeBuilder<TargetVersion> builder) {
                builder.HasAlternateKey(v => v.Name);
                builder.HasAlternateKey(v => v.VersionNumber);
            }

        }

    }

}