using System.ComponentModel.DataAnnotations;
using Disunity.Store.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Areas.Orgs.Models {

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

        public static void OnModelCreating(ModelBuilder builder) {
            builder.Entity<OrgMember>().HasKey(m => new {m.UserId, m.OrgId});
        }

    }

}