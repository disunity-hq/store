using System.ComponentModel.DataAnnotations;
using Disunity.Store.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Mods.Models {

    public class ModVersionDownloadEvent : TrackableModel {

        [Required] public int ModVersionId { get; set; }

        public ModVersion ModVersion { get; set; }

        [DataType(DataType.ImageUrl)] public string SourceIp { get; set; }

        public int TotalDownloads { get; set; }
        public int CountedDownloads { get; set; }

        public static void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ModVersionDownloadEvent>().HasKey(e => new {SourceIP = e.SourceIp, e.ModVersionId});
            builder.Entity<ModVersionDownloadEvent>().Property(e => e.TotalDownloads).HasDefaultValue(1);
            builder.Entity<ModVersionDownloadEvent>().Property(e => e.CountedDownloads).HasDefaultValue(1);
        }

    }

}