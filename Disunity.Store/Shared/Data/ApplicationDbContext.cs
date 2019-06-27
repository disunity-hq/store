using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Areas.Mods.Models;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Areas.Targets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Disunity.Store.Shared.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserIdentity>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApplicationDbContext> _logger;

        static ApplicationDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        }

        public DbSet<Org> Orgs { get; set; }
        public DbSet<OrgMember> OrgMembers { get; set; }

        public DbSet<Mod> Mods { get; set; }
        public DbSet<ModVersion> ModVersions { get; set; }
        public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvents { get; set; }

        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetVersion> TargetVersions { get; set; }

        private class SavedChanges
        {
            public IList<object> Added { get; set; }
            public IList<object> Modified { get; set; }
            public IList<object> Deleted { get; set; }

            public SavedChanges()
            {
                Added = new List<object>();
                Modified = new List<object>();
                Deleted = new List<object>();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ForNpgsqlHasEnum<OrgMemberRole>();

            // Configure all models
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var savedChanges = OnBeforeSave();
            var changes = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSave(savedChanges);
            return changes;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var savedChanges = OnBeforeSave();
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            OnAfterSave(savedChanges);
            return changes;
        }

        private SavedChanges OnBeforeSave()
        {
            var changes = new SavedChanges();
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if (entry.Entity is IBeforeDelete beforeDelete) beforeDelete.OnBeforeDelete(_serviceProvider);
                        changes.Deleted.Add(entry.Entity);
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IBeforeUpdate beforeUpdate) beforeUpdate.OnBeforeUpdate(_serviceProvider);
                        changes.Modified.Add(entry.Entity);
                        break;
                    case EntityState.Added:
                        if (entry.Entity is IBeforeCreate beforeCreate) beforeCreate.OnBeforeCreate(_serviceProvider);
                        changes.Added.Add(entry.Entity);
                        break;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IBeforeSave beforeSave) beforeSave.OnBeforeSave(_serviceProvider);
                }
            }

            return changes;
        }

        private void OnAfterSave(SavedChanges changes)
        {
            foreach (var entity in changes.Added)
            {
                if (entity is IAfterCreate afterCreate) afterCreate.OnAfterCreate(_serviceProvider);
            }

            foreach (var entity in changes.Modified)
            {
                if (entity is IAfterUpdate afterUpdate) afterUpdate.OnAfterUpdate(_serviceProvider);
            }

            foreach (var entity in changes.Deleted)
            {
                if (entity is IAfterDelete afterDelete) afterDelete.OnAfterDelete(_serviceProvider);
            }
        }
    }
}