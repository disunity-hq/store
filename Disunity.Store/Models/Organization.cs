using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Disunity.Store.Models
{
  public class Organization
  {

    public int ID { get; set; }
    public string Name { get; set; }

    public List<OrgMember> Members { get; set; }

    public List<Mod> Mods { get; set; }
  }
}
