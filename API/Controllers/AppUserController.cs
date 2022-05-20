using System.Threading.Tasks;
using API.Middleware;
using Application.Interfaces;
using Application.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class AppUserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserController(
            UserManager<AppUser> userManager,
            IUserAccessor userAccessor
            ) : base(userAccessor)

        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> list()
        {
            var officeId = _userAccessor.GetOfficeId();
            return HandleResult(await Mediator.Send(new List.Query{officeId = officeId}));

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpGet("lookupusername")]
        public async Task<IActionResult> LookupUsernameForCreate(string userName)
        {
            return HandleResult(await Mediator.Send(new LookupUserName.Command{ UserName = userName }));
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppUserDTO appUserDTO)
        {
            return HandleResult(await Mediator.Send(new Create.Command { appUserDTO = appUserDTO }));
        }
        [HttpPut]
        public async Task<IActionResult> Edit(AppUserDTO appUserDTO)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { appUserDTO = appUserDTO }));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }


    }
}