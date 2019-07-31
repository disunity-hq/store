namespace Disunity.Store.Exceptions {

    public class CantChangeOrgOwnerException : BaseApiException {

        public CantChangeOrgOwnerException(string message, string name = null, string context = null) : base(message, name, context) { }

    }

}