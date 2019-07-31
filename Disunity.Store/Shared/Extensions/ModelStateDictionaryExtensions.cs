using Disunity.Store.Exceptions;

using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Disunity.Store.Extensions {

    public static class ModelStateDictionaryExtensions {

        public static void AddApiException(this ModelStateDictionary modelState, 
                                           BaseApiException exception) {
            modelState.TryAddModelException("", exception);
        }

    }

}