using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Disunity.Store.Areas.Identity.Models;
using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Disunity.Store.Models
{
  public enum OrgMemberRole
  {
    Owner,
    Member
  }

  public class OrgMember
  {
    [Required]
    public string UserId { get; set; }
    public UserIdentity User { get; set; }

    [Required]
    public int OrgId { get; set; }
    public Organization Org { get; set; }

    [Required]
    public OrgMemberRole Role { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<OrgMember>().HasKey(m => new { m.UserId, m.OrgId });
    }

  }
}
