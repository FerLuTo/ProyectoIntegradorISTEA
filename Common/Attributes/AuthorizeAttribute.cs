using Entities.Enum;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }

        /// <summary>
        /// Método para validar si el usuario está autorizado o no,
        /// en el caso de no estar logueado o que el rol
        /// no se el que corresponde devuelve "Unauthorized" y StatusCode 401 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var account = (Account)filterContext.HttpContext.Items["Account"];
            if (account is null)
            {
                filterContext.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
