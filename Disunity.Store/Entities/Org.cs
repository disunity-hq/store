using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Data.Hooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class Org : TrackableModel {

        public int Id { get; set; }
        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        public string Slug { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }

        [OnBeforeCreate(typeof(UserIdentity))]
        public static void OnBeforeCreateUser(EntityEntry entityEntry, ApplicationDbContext context) {
            if (!(entityEntry.Entity is UserIdentity user)) {
                return;
            }

            context.Orgs.Add(new Org() {
                DisplayName = user.UserName,
                Members = new List<OrgMember>()
                    {new OrgMember() {Role = OrgMemberRole.Owner, User = user}}
            });
        }

        public class OrgConfiguration : IEntityTypeConfiguration<Org> {

            public void Configure(EntityTypeBuilder<Org> builder) {
                builder.HasAlternateKey(o => o.DisplayName);
                builder.HasIndex(o => o.Slug).IsUnique();

                builder.HasMany(o => o.Mods)
                       .WithOne(m => m.Owner);
            }

        }

    }

}