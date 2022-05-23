using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.User
{
    public class List
    {
        public class Query : IRequest<Result<IList<AppUserDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<IList<AppUserDTO>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _userManager = userManager;
                _unitOfWork = unitOfWork;

            }
            public async Task<Result<IList<AppUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var appUsersOfficeRoles = await _unitOfWork.Users.getAppUsersOfficeRolesByOffice(_userAccessor.GetOfficeId()).OrderBy(q => q.AppUser.CreateDate).ToListAsync();

                var users = _mapper.Map<IList<AppUserOfficeRole>, IList<AppUserDTO>>(appUsersOfficeRoles);


                return Result<IList<AppUserDTO>>.Success(users);
            }
        }
    }
}