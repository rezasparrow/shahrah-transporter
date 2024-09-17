using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Linq;

namespace Shahrah.Transporter.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            return base.BadRequest(modelState.Keys.SelectMany(k => ModelState[k]?.Errors)
                .Select(m => m.ErrorMessage)
                .ToArray());
        }
    }
}