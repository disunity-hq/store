using System.ComponentModel.DataAnnotations;
using Disunity.Store.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disunity.Store.Areas.Orgs.Models
{
    public enum OrgMemberRole
    {
        Owner,
        Member
    }

    public class OrgMember
    {
        [Required] public string UserId { get; set; }

        public UserIdentity User { get; set; }

        [Required] public int OrgId { get; set; }

        public Org Org { get; set; }

        [Required] public OrgMemberRole Role { get; set; }

        public class OrgMemberConfiguration : IEntityTypeConfiguration<OrgMember>
        {
            public void Configure(EntityTypeBuilder<OrgMember> builder)
            {
                builder.HasKey(m => new {m.UserId, m.OrgId});
            }
        }
    }

}