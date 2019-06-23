using System;

namespace Disunity.Store.Util
{
  public interface ITrackableModel
  {
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
  }
}
