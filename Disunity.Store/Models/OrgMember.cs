using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Models {

    public enum OrgMemberRole {

        Owner,
        Member

    }

    public class OrgMember {

        [Required] public string UserId { get; set; }

        public UserIdentity User { get; set; }

        [Required] public int OrgId { get; set; }

        public Org Org { get; set; }

        [Required] public OrgMemberRole Role { get; set; }

        public class OrgMemberConfiguration : IEntityTypeConfiguration<OrgMember> {

            public void Configure(EntityTypeBuilder<OrgMember> builder) {
                builder.HasKey(m => new {m.UserId, m.OrgId});

                builder.HasOne(m => m.Org)
                       .WithMany(o => o.Members)
                       .HasForeignKey(m => m.OrgId);

                builder.HasOne(m => m.User)
                       .WithMany(u => u.Orgs)
                       .HasForeignKey(m => m.UserId);
            }

        }

    }

}