namespace Disunity.Store.Exceptions {

    public class NoSuchUserException : BaseApiException {

        public NoSuchUserException(string message, string name = null, string context = null) : base(message, name, context) { }

    }

}