using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Store.Entities;
using Disunity.Store.Shared.Data.Hooks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Npgsql;


namespace Disunity.Store.Shared.Data {

    public class ApplicationDbContext : IdentityDbContext<UserIdentity> {

        private readonly HookManagerContainer _hooks;
        private readonly ILogger<ApplicationDbContext> _logger;

        static ApplicationDbContext() {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    HookManagerContainer hooks,
                                    ILogger<ApplicationDbContext> logger) : base(options) {

            _hooks = hooks;
            _hooks.InitializeForAll(this);
            _logger = logger;
        }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetVersion> TargetVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ForNpgsqlHasEnum<OrgMemberRole>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            var savedChanges = _hooks.BeforeSave(this);
            var changes = base.SaveChanges(acceptAllChangesOnSuccess);
            _hooks.AfterSave(this, savedChanges);
            return changes;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                                         CancellationToken cancellationToken =
                                                             default(CancellationToken)) {
            var savedChanges = _hooks.BeforeSave(this);
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            _hooks.AfterSave(this, savedChanges);
            return changes;
        }

    }

}