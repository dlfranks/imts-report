using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class LookupUserName
    {
        public class Command : IRequest<Result<LookupUserNameDTO>>
        {
            public string Email { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<LookupUserNameDTO>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IUserAccessor userAccessor, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userAccessor = userAccessor;
            }

            public async Task<Result<LookupUserNameDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<string> errmsg = new List<string>();
                List<string> succmsg = new List<string>();
                AppUser appUser = null;
                LookupUserNameDTO lookupUserNameDTO = new LookupUserNameDTO();
                appUser = await _userManager.FindByEmailAsync(request.Email);
                if (appUser != null)
                {
                    var appUserOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(appUser.Id, _userAccessor.GetOfficeId());
                    if (appUserOfficeRole == null)
                    {
                        var msg = "An existing user was found, if you would like to add this existing user to your office, continue.";
                        succmsg.Add(msg);

                    }
                    else
                    {
                        var msg = "A user with the email already exists in this office.";
                        errmsg.Add(msg);
                    }
                    var appUserDTO = new AppUserDTO();
                    if (appUser != null) appUserDTO = _mapper.Map<AppUserDTO>(appUser);

                    lookupUserNameDTO.appUserDTO = appUserDTO;

                }
                else
                {
                    //new user
                    succmsg.Add("Username is available, continue to create the user.");
                    
                }

                
                //default role for creating
                //appUserDTO.RoleName = OfficeRoleEnum.User.ToString().ToLower();
                //appUserDTO.Password = "";

                var strerrmsg = ""; if (errmsg.Count > 0) foreach (var s in errmsg) strerrmsg += s + "/n";
                var strsuccmsg = ""; if (succmsg.Count > 0) foreach (var s in succmsg) strsuccmsg += s + "/n";
                lookupUserNameDTO.isValidToCreate = errmsg.Count == 0;
                lookupUserNameDTO.errmsg = strerrmsg;
                lookupUserNameDTO.succmsg = strsuccmsg;

                if(errmsg.Count == 0)
                    return Result<LookupUserNameDTO>.Success(lookupUserNameDTO);
                else
                    return Result<LookupUserNameDTO>.Failure(strerrmsg);

            }
        }
    }
}