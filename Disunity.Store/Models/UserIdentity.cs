using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;


namespace Disunity.Store.Models {

    public class UserIdentity : IdentityUser {

        public IList<OrgMember> Orgs { get; set; }

    }

}