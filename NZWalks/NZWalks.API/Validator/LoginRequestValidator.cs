using FluentValidation;

namespace NZWalks.API.Validator
{
    public class LoginRequestValidator : AbstractValidator<Model.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.username).NotEmpty();
            RuleFor(x => x.password).NotEmpty();
        }
    }
}
