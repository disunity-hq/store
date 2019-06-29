using System.Collections.Generic;
using Disunity.Store.Areas.Mods.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Disunity.Store.Shared.Data;

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

    }

}