using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace API.Extensions.Filters
{
    public class UserActionFilter : ActionFilterAttribute
    {
        private readonly UserService _userService;
        private readonly ILogger<UserActionFilter> _logger;

        public UserActionFilter(UserService userService, ILogger<UserActionFilter> logger)
        {
            _logger = logger;
            _userService = userService;

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUserSettings = await _userService.CreateUserSettings();
            if (currentUserSettings == null)
            {
                var error = "Failed to set up the CurrentUserSettings object to the HttpContext";
                _logger.LogError(error);
                context.Result = new JsonResult(new { message = "Unauthorized: " + error }) { StatusCode = StatusCodes.Status401Unauthorized };
            }else{
                _logger.LogInformation("Set the CurrentUserSettings");
                await next();
            }
        }
    }
}