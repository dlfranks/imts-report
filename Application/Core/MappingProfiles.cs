using Application.User;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, AppUserDTO>();
            CreateMap<AppUserOfficeRole, AppUserDTO>()
                .ForMember(q => q.Id, s => s.MapFrom(r => r.AppUser.Id))
                .ForMember(q => q.FirstName, s => s.MapFrom(r => r.AppUser.FirstName))
                .ForMember(q => q.LastName, s => s.MapFrom(r => r.AppUser.LastName))
                .ForMember(q => q.IsImtsUser, s => s.MapFrom(r => r.AppUser.IsImtsUser))
                .ForMember(q => q.ImtsUserName, s => s.MapFrom(r => r.AppUser.ImtsUserName))
                .ForMember(q => q.ImtsEmployeeId, s => s.MapFrom(r => r.AppUser.ImtsEmployeeId))
                .ForMember(q => q.Email, s => s.MapFrom(r => r.AppUser.Email))
                .ForMember(q => q.MainOfficeId, s => s.MapFrom(r => r.AppUser.MainOfficeId))
                .ForMember(q => q.UserName, s => s.MapFrom(r => r.AppUser.UserName))
                .ForMember(q => q.RoleName, s => s.MapFrom(r => r.Role.RoleName));

        }
    }
}