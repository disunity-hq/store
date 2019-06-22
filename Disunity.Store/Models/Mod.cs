using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
  }
}
