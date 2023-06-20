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
        /// Method to validate if the user is authorized or not,
        /// in the case of not being logged in or that the role
        /// not the correct one returns "Unauthorized" and StatusCode 401 
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
