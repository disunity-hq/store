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
            var changes =  base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSave();
            return changes;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                                   CancellationToken cancellationToken = default(CancellationToken)) {
            OnBeforeSave();
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            OnAfterSave();
            return changes;
        }

        private void OnBeforeSave() {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries) {
                if (entry.Entity is IAutoModel<ApplicationDbContext> autoModel)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            autoModel.OnBeforeDelete(this);
                            break;
                        case EntityState.Modified:
                            autoModel.OnBeforeUpdate(this);
                            break;
                        case EntityState.Added:
                            autoModel.OnBeforeCreate(this);
                            break;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        autoModel.OnBeforeSave(this);
                    }
                }
            }
        }

        private void OnAfterSave()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries) {
                if (entry.Entity is IAutoModel<ApplicationDbContext> autoModel)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            autoModel.OnAfterDelete(this);
                            break;
                        case EntityState.Modified:
                            autoModel.OnAfterUpdate(this);
                            break;
                        case EntityState.Added:
                            autoModel.OnAfterCreate(this);
                            break;
                    }

                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        autoModel.OnAfterSave(this);
                    }
                }
            }
        }
    }

}