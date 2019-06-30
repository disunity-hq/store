using System;
using System.Collections.Generic;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Areas.Mods.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Disunity.Store.Shared.Data;
using Disunity.Store.Shared.Data.Hooks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Disunity.Store.Areas.Orgs.Models {

    public class Org : TrackableModel {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }

        public class OrgConfiguration : IEntityTypeConfiguration<Org> {

            public void Configure(EntityTypeBuilder<Org> builder) {
                builder.HasAlternateKey(o => o.Name);

                builder.HasMany(o => o.Mods)
                       .WithOne(m => m.Owner);
            }

        }

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

    }

}