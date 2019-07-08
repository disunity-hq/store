using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class UserIdentity : IdentityUser {

        public IList<OrgMember> Orgs { get; set; }

        public string Slug { get; set; }

        public class UserIdenityConfiguration : IEntityTypeConfiguration<UserIdentity> {

            public void Configure(EntityTypeBuilder<UserIdentity> builder) {
                builder.HasIndex(u => u.Slug).IsUnique();
            }

        }

    }

}