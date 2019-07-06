using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Disunity.Store.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Mods.Models {

    public class ModVersion : TrackableModel {

        public int Id { get; set; }

        public int ModId { get; set; }
        public Mod Mod { get; set; }

        [Required] [MaxLength(128)] public string Name { get; set; }

        public bool IsActive { get; set; }
        public int Downloads { get; set; }

        [Required] [MaxLength(16)] public string VersionNumber { get; set; }

        [Required] [MaxLength(1024)] public string WebsiteUrl { get; set; }

        [Required] [MaxLength(256)] public string Description { get; set; }

        [MaxLength(1024)]
        [Required]
        [DataType(DataType.Upload)]
        public string FileUrl { get; set; }

        [MaxLength(1024)]
        [Required]
        [DataType(DataType.ImageUrl)]
        public string IconUrl { get; set; }

        public List<ModVersion> Dependencies { get; set; }

        public static void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ModVersion>().Property(v => v.IsActive).HasDefaultValue(true);
            builder.Entity<ModVersion>().Property(v => v.Downloads).HasDefaultValue(0);
        }

    }

}