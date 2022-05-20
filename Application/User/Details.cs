using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
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
            public Handler(UserManager<AppUser> userManager, IMapper mapper)
            {
                _mapper = mapper;
                _userManager = userManager;

            }

            public async Task<Result<AppUserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userDto = await _userManager.Users.Where(q => q.Id == request.Id)
                .ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                return Result<AppUserDTO>.Success(userDto);
            }
        }
    }
}