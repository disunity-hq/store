using Microsoft.AspNetCore.Mvc;

namespace Disunity.Store.Areas.API.v1.Controllers {

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase {

        [HttpGet]
        public IActionResult Index() {
            return new JsonResult(new {Method = "GET", slug = "/"});
        }

        [HttpPost]
        public IActionResult PostIndex() {
            return new JsonResult(new {Method = "POST", slug = "/"});
        }

        [HttpPost("Edit")]
        public IActionResult DoSomeSTuff() {
            return new JsonResult(new {Method = "POST", slug = "/edit"});
        }

    }

}