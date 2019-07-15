using System.Linq;
using System.Threading.Tasks;

using BindingAttributes;

using Disunity.Store.Entities;

using Microsoft.AspNetCore.Hosting;


namespace Disunity.Store.Shared.Data.Seeds {

    [AsScoped(typeof(ISeeder))]
    public class UnityVersionSeed : ISeeder {

        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _context;

        private static readonly string[] _versions = {
            "3.4.0", "3.4.1", "3.4.2", "3.5.0", "3.5.1", "3.5.2", "3.5.3", "3.5.4", "3.5.5", "3.5.6",
            "3.5.7", "4.0.0", "4.0.1", "4.1.0", "4.1.2", "4.1.3", "4.1.4", "4.1.5", "4.2.0", "4.2.1",
            "4.2.2", "4.3.0", "4.3.1", "4.3.2", "4.3.3", "4.3.4", "4.5.0", "4.5.1", "4.5.2", "4.5.3",
            "4.5.4", "4.5.5", "4.6.0", "4.6.1", "4.6.2", "4.6.3", "4.6.4", "4.6.5", "4.6.6", "4.6.7",
            "4.6.8", "4.6.9", "4.7.0", "4.7.1", "4.7.2", "5.0.0", "5.0.1", "5.0.2", "5.0.3", "5.0.4",
            "5.1.0", "5.1.1", "5.1.2", "5.1.3", "5.1.4", "5.1.5", "5.2.0", "5.2.1", "5.2.2", "5.2.3",
            "5.2.4", "5.2.5", "5.3.0", "5.3.1", "5.3.2", "5.3.3", "5.3.4", "5.3.5", "5.3.6", "5.3.7",
            "5.3.8", "5.4.0", "5.4.1", "5.4.2", "5.4.3", "5.4.4", "5.4.5", "5.4.6", "5.5.0", "5.5.1",
            "5.5.2", "5.5.3", "5.5.4", "5.5.5", "5.5.6", "5.6.0", "5.6.1", "5.6.2", "5.6.3", "5.6.4",
            "5.6.5", "5.6.6", "5.6.7",
            "2017.1.0", "2017.1.1", "2017.1.2", "2017.1.3", "2017.1.4", "2017.1.5", "2017.2.0",
            "2017.2.1", "2017.2.2", "2017.2.3", "2017.2.4", "2017.2.5", "2017.3.0", "2017.3.1",
            "2017.4.1", "2017.4.2", "2017.4.3", "2017.4.4", "2017.4.5", "2017.4.6", "2017.4.7",
            "2017.4.8", "2017.4.9", "2017.4.10", "2017.4.11", "2017.4.12", "2017.4.13", "2017.4.14",
            "2017.4.15", "2017.4.16", "2017.4.17", "2017.4.18", "2017.4.19", "2017.4.20", "2017.4.21",
            "2017.4.22", "2017.4.23", "2017.4.24", "2017.4.25", "2017.4.26", "2017.4.27", "2017.4.28",
            "2017.4.29", "2017.4.30",
            "2018.1.0", "2018.1.1", "2018.1.2", "2018.1.3", "2018.1.4", "2018.1.5", "2018.1.6",
            "2018.1.7", "2018.1.8", "2018.1.9", "2018.2.0", "2018.2.1", "2018.2.2", "2018.2.3",
            "2018.2.4", "2018.2.5", "2018.2.6", "2018.2.7", "2018.2.8", "2018.2.9", "2018.2.10",
            "2018.2.11", "2018.2.12", "2018.2.13", "2018.2.14", "2018.2.15", "2018.2.16", "2018.2.17",
            "2018.2.18", "2018.2.19", "2018.2.20", "2018.2.21", "2018.3.0", "2018.3.1", "2018.3.2",
            "2018.3.3", "2018.3.4", "2018.3.5", "2018.3.6", "2018.3.7", "2018.3.8", "2018.3.9",
            "2018.3.10", "2018.3.11", "2018.3.12", "2018.3.13", "2018.3.14", "2018.4.0", "2018.4.1",
            "2018.4.2", "2018.4.3", "2018.4.4",
            "2019.1.0", "2019.1.1", "2019.1.2", "2019.1.3", "2019.1.4", "2019.1.5", "2019.1.6",
            "2019.1.7", "2019.1.8", "2019.1.9", "2019.1.10",
        };

        public UnityVersionSeed(ApplicationDbContext context, IHostingEnvironment env) {
            _context = context;
            _env = env;
        }

        public bool ShouldSeed() {
            return _env.IsDevelopment() && !_context.UnityVersions.Any();
        }

        public Task Seed() {
            foreach (var version in _versions) {
                new UnityVersion() {VersionNumber = (VersionNumber)version};
            }
            return _context.SaveChangesAsync();
        }

    }

}