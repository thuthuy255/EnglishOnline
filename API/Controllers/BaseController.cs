using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int? UserId
        {
            get
            {
                var claim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                return claim != null ? int.Parse(claim) : null;
            }
        }
    }
}
