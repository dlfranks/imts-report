using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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
            private readonly IUnitOfWork _unitOfWork;

            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly ILogger<Application.User.Delete> _logger;

            public Handler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IUserAccessor userAccessor, ILogger<Application.User.Delete> logger)
            {
                _logger = logger;
                _userAccessor = userAccessor;
                _userManager = userManager;
                _unitOfWork = unitOfWork;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var officeId = _userAccessor.GetOfficeId();
                var userOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(request.Id, officeId);

                if (userOfficeRole == null) return Result<Unit>.Failure("User Not Found");
                try
                {
                    await _unitOfWork.Users.removeAppUserOfficeRole(request.Id, officeId);
                    await _unitOfWork.Commit();

                    var identityResult = await _userManager.DeleteAsync(userOfficeRole.AppUser);

                    if (identityResult.Succeeded) return Result<Unit>.Success(Unit.Value);

                }
                catch (Exception ex)
                {
                    Result<Unit>.Failure("Sql Error: " + ex);
                }
                return Result<Unit>.Failure("Sql Error");
            }
        }
    }


}