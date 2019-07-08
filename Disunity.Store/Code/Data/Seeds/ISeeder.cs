using System.Threading.Tasks;


namespace Disunity.Store.Code.Data.Seeds {

    public interface ISeeder {

        bool ShouldSeed();
        Task Seed();

    }

}