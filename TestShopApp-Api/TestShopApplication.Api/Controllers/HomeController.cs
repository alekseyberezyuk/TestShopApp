using Microsoft.AspNetCore.Mvc;

namespace TestShopApplication.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            return Redirect("swagger");
        }
    }
}
