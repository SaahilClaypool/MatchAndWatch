using Infrastructure.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;

namespace App {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context) {
            var user = (ApplicationUser)context.HttpContext.Items["User"];
            if (user == null) {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
