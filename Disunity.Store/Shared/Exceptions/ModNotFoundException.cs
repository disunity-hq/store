using System;


namespace Disunity.Store.Exceptions {

    public class ModNotFoundException : BaseApiException {

        public ModNotFoundException(string message, string name = null, string context = null)
            : base(message, name, context) { }

    }

}