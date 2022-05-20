using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Repository;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UnitOfWork _unitOfWork;

            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;

            public Handler(UnitOfWork unitOfWork, UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _userManager = userManager;
                _unitOfWork = unitOfWork;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUser = _userAccessor.GetUserSettings();
                
                var userOfficeRole = await _unitOfWork.Users.getAppUsersOfficeRoleByUserIdAndOfficeId(request.Id, currentUser.currentOfficeId);

                if (userOfficeRole == null) return Result<Unit>.Failure("User Not Found");

                await _unitOfWork.Users.removeRoleFromUser(request.Id, currentUser.currentOfficeId, userOfficeRole.Role.RoleName);

                var identityResult = await _userManager.DeleteAsync(userOfficeRole.AppUser);

                if (identityResult.Succeeded) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Sql Error");
            }
        }
    }


}