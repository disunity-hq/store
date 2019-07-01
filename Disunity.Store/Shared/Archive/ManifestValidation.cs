using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Disunity.Store.Shared.Archive {

    public partial class Manifest {

        public void Validate(IFormFile formFile,
                             ModelStateDictionary modelState) {
            modelState.AddModelError(formFile.Name, "Some Error");
        }

    }

}