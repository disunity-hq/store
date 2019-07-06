using System.Collections.Generic;
using Disunity.Store.Areas.Mods.Models;

namespace Disunity.Store.Areas.Orgs.Models {

    public class Org {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }

    }

}