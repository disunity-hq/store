using System;

namespace Disunity.Store.Models
{
  public interface ITrackableModel
  {
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
  }
}
