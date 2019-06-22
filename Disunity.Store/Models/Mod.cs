using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Models
{
  public class Mod : TrackableModel
  {
    public int ID { get; set; }

    public int? OwnerId { get; set; }
    public Organization Owner { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; }

    public bool IsActive { get; set; }
    public bool IsDeprecated { get; set; }
    public bool IsPinned { get; set; }

    public int? LatestId { get; set; }
    public ModVersion Latest { get; set; }

    [InverseProperty("Mod")]
    public List<ModVersion> Versions { get; set; }

    public Mod()
    {
      IsActive = true;
      IsDeprecated = false;
      IsPinned = false;
    }

    public static void OnModelCreating(ModelBuilder builder) {
      builder.Entity<Mod>().Property(m => m.IsActive).HasDefaultValue(true);
      builder.Entity<Mod>().Property(m => m.IsDeprecated).HasDefaultValue(false);
      builder.Entity<Mod>().Property(m => m.IsPinned).HasDefaultValue(false);
      
      builder.Entity<Mod>()
        .HasOne(m => m.Owner)
        .WithMany(o => o.Mods)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Mod>()
        .HasOne(m => m.Latest)
        .WithOne(v => v.Mod);

      builder.Entity<Mod>()
        .HasMany(m => m.Versions);
    }
  }
}
