using System.Threading.Tasks;


namespace Disunity.Store.Shared.Data.Seeds {

    public interface ISeeder {

        bool ShouldSeed();
        Task Seed();

    }

}