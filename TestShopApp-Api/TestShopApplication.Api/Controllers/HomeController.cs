using Microsoft.AspNetCore.Mvc;

namespace TestShopApplication.Api.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult Get()
        {
            return Redirect("swagger");
        }
    }
}
