using FluentValidation;

namespace Application.User
{
    public class AppuserValidator :AbstractValidator<AppUserDTO>
    {
        public AppuserValidator()
        {
            RuleFor(q => q.FirstName).NotEmpty();
            RuleFor(q => q.LastName).NotEmpty();
            RuleFor(q => q.Email).NotEmpty().EmailAddress();
            RuleFor(q => q.RoleName).NotEmpty();
        }
    }
}