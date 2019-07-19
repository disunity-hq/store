using System;
using System.Collections.Generic;

using Disunity.Store.Data.Services;

using EFCoreHooks.Attributes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities {

    public class UserIdentity : IdentityUser {

        public IList<OrgMember> Orgs { get; set; }

        public string Slug { get; set; }

        [OnBeforeCreate]
        public static void OnBeforeCreate(UserIdentity user, IServiceProvider services) {
            if (user.Slug == null) {
                var slugifier = services.GetRequiredService<ISlugifier>();
                user.Slug = slugifier.Slugify(user.UserName);
            }
        }

        public class UserIdenityConfiguration : IEntityTypeConfiguration<UserIdentity> {

            public void Configure(EntityTypeBuilder<UserIdentity> builder) {
                builder.HasIndex(u => u.Slug).IsUnique();
            }

        }

    }

}