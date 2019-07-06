using System;
using System.Threading;
using System.Threading.Tasks;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Areas.Orgs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Disunity.Store.Shared.Data {

    public class ApplicationDbContext : IdentityDbContext<UserIdentity> {

        static ApplicationDbContext() {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ForNpgsqlHasEnum<OrgMemberRole>();

            OrgMember.OnModelCreating(builder);

            Mod.OnModelCreating(builder);
            ModVersion.OnModelCreating(builder);
            ModVersionDownloadEvent.OnModelCreating(builder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            OnBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                                   CancellationToken cancellationToken = default(CancellationToken)) {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSave() {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries) {
                if (entry.Entity is ITrackableModel trackable) {
                    var now = DateTime.UtcNow;
                    switch (entry.State) {
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.UpdatedAt = now;
                            break;
                    }
                }
            }
        }

    }

}