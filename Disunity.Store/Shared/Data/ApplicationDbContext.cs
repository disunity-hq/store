using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Store.Entities;

using EFCoreHooks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Npgsql;


namespace Disunity.Store.Shared.Data {

    public class ApplicationDbContext : HookedIdentityDbContext<UserIdentity> {

        private readonly ILogger<ApplicationDbContext> _logger;

        static ApplicationDbContext() {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    HookManagerContainer hooks,
                                    ILogger<ApplicationDbContext> logger) : base(options, hooks) {

            _logger = logger;
        }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModDependency> ModDependencies { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }
        public DbSet<ModTargetCompatibility> ModTargetCompatibilities { get; set; }
        public DbSet<ModDisunityCompatibility> ModDisunityCompatibilities { get; set; }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetVersion> TargetVersions { get; set; }
        public DbSet<TargetVersionCompatibility> TargetVersionCompatibilities { get; set; }

        public DbSet<DisunityVersion> DisunityVersions { get; set; }
        public DbSet<DisunityVersionCompatibility> DisunityVersionCompatibilities { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ForNpgsqlHasEnum<OrgMemberRole>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }

}