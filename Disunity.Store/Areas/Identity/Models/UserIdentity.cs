using System.Collections.Generic;
using Disunity.Store.Areas.Orgs.Models;
using Microsoft.AspNetCore.Identity;

namespace Disunity.Store.Areas.Identity.Models {

    public class UserIdentity : IdentityUser {

        public IList<OrgMember> Orgs { get; set; }

    }

}