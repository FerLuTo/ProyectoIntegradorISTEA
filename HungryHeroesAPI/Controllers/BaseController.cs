using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HungryHeroesAPI.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // Devuelve la cuenta actual autenticada (Si no inicio sesión devuelve nulo)
        public Account Account => (Account)HttpContext.Items["Account"];

        public Product Product => (Product)HttpContext.Items["Product"];
        public UserBusiness UserBusiness => (UserBusiness)HttpContext.Items["UserBusiness"];
        public UserClient UserClient => (UserClient)HttpContext.Items["UserClient"];
        public Sale Sale => (Sale)HttpContext.Items["Sale"];
    }

}
