using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Disunity.Store.Policies {

    public static class OperationsExtensions {

        public static OperationAuthorizationRequirement ToRequirement(this Operation op) {
            return new OperationAuthorizationRequirement() {Name = op.ToString()};
        }

    }

}