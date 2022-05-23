using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class Details
    {
        public class Query : IRequest<Result<AppUserDTO>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AppUserDTO>>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            private readonly IUnitOfWork _unitOfWork;
            public Handler(UserManager<AppUser> userManager, IUserAccessor userAccessor, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _userAccessor = userAccessor;
                _mapper = mapper;
                _userManager = userManager;

            }

            public async Task<Result<AppUserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var officeId = _userAccessor.GetOfficeId();
                var appUserOfficeRole = await _unitOfWork.Users.getAppUsersOfficeRolesByUserAndOffice(request.Id, officeId);
                var appUserDTO = _mapper.Map<AppUserDTO>(appUserOfficeRole);
                
                return Result<AppUserDTO>.Success(appUserDTO);
            }
        }
    }
}