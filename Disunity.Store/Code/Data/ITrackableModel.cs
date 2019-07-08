using System;


namespace Disunity.Store.Code.Data {

    public interface ITrackableModel {

        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }

    }

}