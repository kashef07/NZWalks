using FluentValidation;

namespace NZWalks.API.Validator
{
    public class UpdateWalkRequestValidtor : AbstractValidator<Model.DTO.UpdateWalkRequest>
    {
        public UpdateWalkRequestValidtor()
        {

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
