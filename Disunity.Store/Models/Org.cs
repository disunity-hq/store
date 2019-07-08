using System.Collections.Generic;

using Disunity.Store.Code.Data;
using Disunity.Store.Code.Data.Hooks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Models {

    public class Org : TrackableModel {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }

        [OnBeforeCreate(typeof(UserIdentity))]
        public static void OnBeforeCreateUser(EntityEntry entityEntry, ApplicationDbContext context) {
            if (!(entityEntry.Entity is UserIdentity user)) {
                return;
            }

            context.Orgs.Add(new Org() {
                Name = user.UserName,
                Members = new List<OrgMember>()
                    {new OrgMember() {Role = OrgMemberRole.Owner, User = user}}
            });
        }

        public class OrgConfiguration : IEntityTypeConfiguration<Org> {

            public void Configure(EntityTypeBuilder<Org> builder) {
                builder.HasAlternateKey(o => o.Name);

                builder.HasMany(o => o.Mods)
                       .WithOne(m => m.Owner);
            }

        }

    }

}