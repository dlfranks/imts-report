using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable

namespace API.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Policy { get; set; }
        public string Role { get; set; }
        public AuthorizeAttribute()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Used the userId because it gets to the Onauthentication event before setting the UserSetting
            var userId = context.HttpContext.Items["UserId"];
            var roleName = context.HttpContext.Items["RoleName"];
            var issuedId = context.HttpContext?.Request.RouteValues.SingleOrDefault(q => q.Key == "id").Value?.ToString();
            var issuedController = context.HttpContext?.Request.RouteValues.SingleOrDefault(q => q.Key == "controller").Value;

            if (userId == null || roleName == null || string.IsNullOrWhiteSpace(issuedController.ToString()))
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            if (issuedController.ToString().ToLower() == "appuser")
            {
                if (!string.IsNullOrWhiteSpace(Role) && Role.ToLower() != roleName.ToString().ToLower())
                {
                    context.Result = new JsonResult(new { message = "You don't have a permission." }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Policy) && Policy == "IsCurrentUser")
                    {
                        if (userId.ToString() == issuedId)
                        {
                            context.Result = new JsonResult(new { message = "You cannot delete yourself." }) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                    }
                }
            }
            
        }
    }
}