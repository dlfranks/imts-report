using System.Threading.Tasks;
using API.Middleware;
using API.Services;
using Application.Interfaces;
using Application.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Role = "administrator")]
    public class AppUserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserController(
            UserManager<AppUser> userManager,
            IUserAccessor userAccessor,
            UserService userService
            ) : base(userAccessor, userService)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> list()
        {
            var officeId = _userAccessor.GetOfficeId();
            return HandleResult(await Mediator.Send(new List.Query()));

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpGet("lookupusername")]
        public async Task<IActionResult> LookupUsernameForCreate(string email)
        {
            return HandleResult(await Mediator.Send(new LookupUserName.Command { Email = email }));
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppUserDTO appUserDTO)
        {
            return HandleResult(await Mediator.Send(new Create.Command { appUserDTO = appUserDTO, mode = Application.Core.FormViewMode.Create }));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, AppUserDTO appUserDTO)
        {
            appUserDTO.Id = id;
            return HandleResult(await Mediator.Send(new Create.Command { appUserDTO = appUserDTO, mode = Application.Core.FormViewMode.Edit }));
        }
        //[ClaimRequirement(MyClaimTypes.Permission, "CanReadResource")]
        [Authorize(Role = "administrator", Policy = "IsCurrentUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }


    }
}