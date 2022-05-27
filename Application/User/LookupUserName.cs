using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class LookupUserName
    {
        public class Command : IRequest<Result<bool>>
        {
            public string UserName { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly UserManager<AppUser> _userManager;

            private readonly IUserAccessor _userAccessor;
            public Handler(IUnitOfWork unitOfWork, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                //if (!(await _userService.permissionForEmployee(PermissionAction.Create, null, autoThrow: false)))
                //return Unauthorized();

                var appUser = await _userManager.FindByNameAsync(request.UserName);

                if (appUser == null)
                    return Result<bool>.Success(true);
                else
                {
                    var result = new Result<bool>();
                    result.Value = false;
                    result.IsSuccess = false;
                    result.Error = "A user with the username already exists in this office, please select another username.";
                    return result;
                }
            }

            
        }
    }
}