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
        public class Query : IRequest<Result<List<AppUserDTO>>>
        {
            public int officeId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AppUserDTO>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;
            public Handler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
            {
                _mapper = mapper;
                _userManager = userManager;
                _unitOfWork = unitOfWork;

            }
            public async Task<Result<List<AppUserDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lst = await _unitOfWork.AppUsers.getAppUsersOfficeRoleByOffice(request.officeId).Select(q => q.AppUser).OrderBy(q => q.CreateDate).ToListAsync();
                
                var users = _mapper.Map<List<AppUserDTO>>(lst);

                return Result<List<AppUserDTO>>.Success(users);
            }
        }
    }
}