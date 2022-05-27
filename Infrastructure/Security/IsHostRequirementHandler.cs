using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;
        public IsHostRequirementHandler(IUnitOfWork unitOfWork, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _unitOfWork = unitOfWork;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var currentUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var officeId = _userAccessor.GetOfficeId();
            if (currentUserId == null) return Task.CompletedTask;

            var currentUser = _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(currentUserId, officeId);
            var issuedUserId = _userAccessor.GetHttpContextAccessor()?.HttpContext?.Request.RouteValues
            .SingleOrDefault(q => q.Key == "id").Value?.ToString();
            var issuedUser = _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(issuedUserId, officeId);

            if(issuedUser == null) return Task.CompletedTask;
            if(currentUserId == issuedUserId) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}