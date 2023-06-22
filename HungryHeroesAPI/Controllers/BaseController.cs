using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HungryHeroesAPI.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public Account Account => (Account)HttpContext.Items["Account"];
    }

}
