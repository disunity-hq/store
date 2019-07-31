namespace Disunity.Store.Exceptions {

    public class DuplicateOrgMemberException : BaseApiException {

        public DuplicateOrgMemberException(string message, string name = null, string context = null) : base(message, name, context) { }

    }

}