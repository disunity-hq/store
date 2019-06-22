using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Disunity.Store.Models
{
  public class ModVersion : TrackableModel
  {
    public int ID { get; set; }

    public int ModId { get; set; }
    public Mod Mod { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int Downloads { get; set; }
    [Required]
    [MaxLength(16)]
    public string VersionNumber { get; set; }
    [Required]
    [MaxLength(1024)]
    public string WebsiteUrl { get; set; }
    [Required]
    [MaxLength(256)]
    public string Description { get; set; }
    [MaxLength(1024)]
    [Required]
    [DataType(DataType.Upload)]
    public string FileURL { get; set; }
    [MaxLength(1024)]
    [Required]
    [DataType(DataType.ImageUrl)]
    public string IconURL { get; set; }

    public List<ModVersion> Dependencies { get; set; }

  }


}
