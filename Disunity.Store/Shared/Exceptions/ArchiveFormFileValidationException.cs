using System;


namespace Disunity.Store.Exceptions {

    public class ArchiveFormFileValidationException : BaseApiException {

        public ArchiveFormFileValidationException(string message, string name = null, string context = null)
            : base(message, name, context) { }

    }

}