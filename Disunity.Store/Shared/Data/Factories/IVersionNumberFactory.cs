using System.Threading.Tasks;

using Disunity.Store.Entities;


namespace Disunity.Store.Shared.Data.Factories {

    public interface IVersionNumberFactory {

        Task<VersionNumber> FindOrCreateVersionNumber(string versionString);
        Task<VersionNumber> FindOrCreateVersionNumber(VersionNumber versionNumber);

    }

}