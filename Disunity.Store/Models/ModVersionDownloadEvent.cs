using System.ComponentModel.DataAnnotations;

namespace Disunity.Store.Models
{
  public class ModVersionDownloadEvent : TrackableModel
  {
    [Required]
    public int ModVersionId { get; set; }
    public ModVersion ModVersion { get; set; }

    [DataType(DataType.ImageUrl)]
    public string SourceIP { get; set; }

    public int TotalDownloads { get; set; }
    public int CountedDownloads { get; set; }
  }
}
