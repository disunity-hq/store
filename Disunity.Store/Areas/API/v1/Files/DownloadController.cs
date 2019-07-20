using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Disunity.Store.Areas.API.v1.Files {

    [ApiController]
    [Route("api/v{version:apiVersion}/files")]
    public class DownloadController:ControllerBase {

        public DownloadController() { }

        /// <summary>
        /// Download a file by it's id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile([FromRoute]string fileId) {
            await Task.CompletedTask;
            return new RedirectResult("", false);
        }

        public void Foo(HttpContext context) {
            
        }

    }

}