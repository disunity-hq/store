using Disunity.Store.Entities;


namespace Disunity.Store.Shared.Data {

    public interface IVersionModel {

        int VersionNumberId { get; set; }

        VersionNumber VersionNumber { get; set; }

    }

}