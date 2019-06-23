using System;
using System.ComponentModel.DataAnnotations;

namespace Disunity.Store.Util
{
  public class TrackableModel : ITrackableModel
  {
    [DataType(DataType.Date)]
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; }
  }
}
