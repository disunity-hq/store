using System.Dynamic;
using System.Linq;
using System.Net;

using Disunity.Store.Exceptions;
using Disunity.Store.Extensions;

using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Areas.API.v1 {

    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase {

        private HttpStatusCode Status = HttpStatusCode.Accepted;

        public ObjectResult ApiResult(HttpStatusCode? status = null) {
            dynamic body = new ExpandoObject();

            if (ModelState.ErrorCount > 0) {
                body.errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.Exception)
                                        .ToList();
            }

            return new ObjectResult(body) {
                StatusCode = (int?) (status ?? Status)
            };
        }

        public ObjectResult BadRequest() {
            return ApiResult(HttpStatusCode.BadRequest);
        }
        
        public ObjectResult BadRequest(BaseApiException exception) {
            ModelState.AddApiException(exception);
            return ApiResult(HttpStatusCode.BadRequest);
        }
        
        public ObjectResult NotFound() {
            return ApiResult(HttpStatusCode.NotFound);
        }
        
        public ObjectResult NotFound(BaseApiException exception) {
            ModelState.AddApiException(exception);
            return ApiResult(HttpStatusCode.NotFound);
        }
        
    }

}