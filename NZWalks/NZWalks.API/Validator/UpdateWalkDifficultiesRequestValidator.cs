using FluentValidation;

namespace NZWalks.API.Validator
{
    public class UpdateWalkDifficultiesRequestValidator : AbstractValidator<Model.DTO.UpdateWalkDifficultiesRequest>
    {
        public UpdateWalkDifficultiesRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
