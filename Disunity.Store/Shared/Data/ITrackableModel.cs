using System;

namespace Disunity.Store.Shared.Data {

    public interface ITrackableModel {

        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }

    }

}