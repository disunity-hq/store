using System;
using System.ComponentModel.DataAnnotations;

using Disunity.Store.Shared.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModVersionDownloadEvent : TrackableModel {

        [Required] public int ModVersionId { get; set; }

        public ModVersion ModVersion { get; set; }

        [DataType(DataType.ImageUrl)] public string SourceIp { get; set; }

        // TODO Use UpdatedAt for this field?
        [DataType(DataType.DateTime)] public DateTime LatestDownload { get; set; }

        public int? TotalDownloads { get; set; }
        public int? CountedDownloads { get; set; }

        public class ModVersionDownloadEventConfiguration : IEntityTypeConfiguration<ModVersionDownloadEvent> {

            public void Configure(EntityTypeBuilder<ModVersionDownloadEvent> builder) {
                builder.HasKey(e => new {SourceIP = e.SourceIp, e.ModVersionId});
                builder.Property(e => e.TotalDownloads).HasDefaultValue(1);
                builder.Property(e => e.CountedDownloads).HasDefaultValue(1);
            }

        }

    }

}