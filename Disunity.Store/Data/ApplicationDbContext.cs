using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disunity.Store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Disunity.Store.Data
{
  public class ApplicationDbContext : IdentityDbContext<UserIdentity>
  {
    public DbSet<Organization> Orgs { get; set; }

    public DbSet<OrgMember> OrgMembers { get; set; }

    public DbSet<Mod> Mod { get; set; }
    public DbSet<ModVersion> ModVersion { get; set; }
    public DbSet<ModVersionDownloadEvent> ModVersionDownloadEvent { get; set; }

    static ApplicationDbContext()
    {
      NpgsqlConnection.GlobalTypeMapper.MapEnum<OrgMemberRole>();
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);

      builder.ForNpgsqlHasEnum<OrgMemberRole>();

      builder.Entity<OrgMember>()
          .HasKey(m => new { m.UserId, m.OrgId });

      builder.Entity<ModVersionDownloadEvent>()
          .HasKey(e => new { e.SourceIP, e.ModVersionId });

      builder.Entity<Mod>().Property(m => m.IsActive).HasDefaultValue(true);
      builder.Entity<Mod>().Property(m => m.IsDeprecated).HasDefaultValue(false);
      builder.Entity<Mod>().Property(m => m.IsPinned).HasDefaultValue(false);


      builder.Entity<ModVersion>().Property(v => v.IsActive).HasDefaultValue(true);
      builder.Entity<ModVersion>().Property(v => v.Downloads).HasDefaultValue(0);

      builder.Entity<ModVersionDownloadEvent>().Property(e => e.TotalDownloads).HasDefaultValue(1);
      builder.Entity<ModVersionDownloadEvent>().Property(e => e.CountedDownloads).HasDefaultValue(1);

      builder.Entity<Mod>()
          .HasOne(m => m.Owner)
          .WithMany(o => o.Mods)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Mod>()
          .HasOne(m => m.Latest)
          .WithOne(v => v.Mod);

      builder.Entity<Mod>()
          .HasMany(m => m.Versions);

    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
      OnBeforeSave();
      return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
      OnBeforeSave();
      return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSave()
    {
      var entries = ChangeTracker.Entries();
      foreach (var entry in entries)
      {
        if (entry.Entity is ITrackableModel trackable)
        {
          var now = DateTime.UtcNow;
          switch (entry.State)
          {
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
