using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Areas.Targets.Models;
using Disunity.Store.Shared.Data.Hooks;
using Disunity.Store.Shared.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Disunity.Store.Shared.Data {

    public class ApplicationDbContext : IdentityDbContext<UserIdentity> {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApplicationDbContext> _logger;
        private readonly HookManagerContainer _hooks;

        static ApplicationDbContext() {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceProvider serviceProvider,
                                    HookManagerContainer hooks)
            : base(options) {
            _serviceProvider = serviceProvider;
            _hooks = hooks;
            _logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            
            _hooks.InitializeForAll(this);
        }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetVersion> TargetVersions { get; set; }

        private class SavedChanges {

            public IList<EntityEntry> Added { get; }
            public IList<EntityEntry> Modified { get; }
            public IList<EntityEntry> Deleted { get; }
            public IList<EntityEntry> Saved { get; }

            public SavedChanges() {
                Added = new List<EntityEntry>();
                Modified = new List<EntityEntry>();
                Deleted = new List<EntityEntry>();
                Saved = new List<EntityEntry>();
            }

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ForNpgsqlHasEnum<OrgMemberRole>();

            // Configure all models
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            var savedChanges = OnBeforeSave();
            var changes = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSave(savedChanges);
            return changes;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
                                                         CancellationToken cancellationToken =
                                                             default(CancellationToken)) {
            var savedChanges = OnBeforeSave();
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            OnAfterSave(savedChanges);
            return changes;
        }

        private SavedChanges OnBeforeSave() {
            var changes = new SavedChanges();
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries) {
                switch (entry.State) {
                    case EntityState.Deleted:
                        _hooks.OnBeforeDelete.ExecuteForEntity(this, entry);
                        changes.Deleted.Add(entry);
                        break;

                    case EntityState.Modified:
                        _hooks.OnBeforeUpdate.ExecuteForEntity(this, entry);
                        changes.Modified.Add(entry);
                        break;

                    case EntityState.Added:
                        _hooks.OnBeforeCreate.ExecuteForEntity(this, entry);
                        changes.Added.Add(entry);
                        break;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified) {
                    _hooks.OnBeforeSave.ExecuteForEntity(this, entry);
                    changes.Saved.Add(entry);
                }
            }

            return changes;
        }

        private void OnAfterSave(SavedChanges changes) {
            foreach (var entity in changes.Added) {
                _hooks.OnAfterCreate.ExecuteForEntity(this, entity);
            }

            foreach (var entity in changes.Modified) {
                _hooks.OnAfterUpdate.ExecuteForEntity(this, entity);
            }

            foreach (var entity in changes.Deleted) {
                _hooks.OnAfterDelete.ExecuteForEntity(this, entity);
            }

            foreach (var entity in changes.Saved) {
                _hooks.OnAfterSave.ExecuteForEntity(this, entity);
            }
        }

    }

}