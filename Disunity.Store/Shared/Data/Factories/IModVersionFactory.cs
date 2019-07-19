using System.Threading.Tasks;

using Disunity.Store.Entities;


namespace Disunity.Store.Data.Factories {

    public interface IModVersionFactory {

        /// <summary>
        /// Creates a new <see cref="ModVersion"/> from the information available in the <see cref="Archive"/>
        /// </summary>
        /// <remarks>
        /// This method does not actually do any of the hard work of storing the archive and creating a url for it
        /// </remarks>
        /// <param name="archive">An <see cref="Archive"/> containing information to build a <see cref="Mod"/></param>
        /// <param name="context">The DbContext on which to search for version numbers</param>
        /// <returns>A new <see cref="ModVersion"/>. Be careful to ensure it is valid before adding to the db</returns>
        Task<ModVersion> FromArchiveAsync(Archive.Archive archive);

    }

}