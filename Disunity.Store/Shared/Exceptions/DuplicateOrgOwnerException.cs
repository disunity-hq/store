namespace Disunity.Store.Exceptions {

    public class DuplicateOrgOwnerException : BaseApiException {

        public DuplicateOrgOwnerException(string message, 
                                          string name = null, 
                                          string context = null) 
            : base(message, name, context) { }
    }

}