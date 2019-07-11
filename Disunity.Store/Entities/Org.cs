using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Disunity.Store.Shared.Data;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class Org : ICreatedAt {

        public int Id { get; set; }
        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        public string Slug { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }

        [OnBeforeCreate(typeof(UserIdentity), WatchDescendants = false)]
        public static void OnBeforeCreateUser(UserIdentity user, ApplicationDbContext context) {

            var org = context.Orgs.FirstOrDefault(o => o.DisplayName == user.UserName);

            if (org == null) {
                context.Orgs.Add(new Org() {
                    DisplayName = user.UserName,
                    Members = new List<OrgMember>()
                        {new OrgMember() {Role = OrgMemberRole.Owner, User = user}}
                });
            }
        }

        public class OrgConfiguration : IEntityTypeConfiguration<Org> {

            public void Configure(EntityTypeBuilder<Org> builder) {
                builder.HasAlternateKey(o => o.DisplayName);
                builder.HasIndex(o => o.Slug).IsUnique();

                builder.HasMany(o => o.Mods)
                       .WithOne(m => m.Owner);
            }

        }

        public DateTime CreatedAt { get; set; }

    }

}