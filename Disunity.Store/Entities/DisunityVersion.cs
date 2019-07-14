using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class DisunityVersion : IVersionModel {

        public int ID { get; set; }

        [DataType(DataType.Url)] public string URL { get; set; }

        [Required] public int VersionNumberId { get; set; }

        public VersionNumber VersionNumber { get; set; }

        [InverseProperty("Version")] public DisunityVersionCompatibility CompatibileUnityVersion { get; set; }

        public class DisunityVersionConfiguration : IEntityTypeConfiguration<DisunityVersion> {

            public void Configure(EntityTypeBuilder<DisunityVersion> builder) {
                builder.HasVersionIndex().IsUnique();
            }

        }

    }

}