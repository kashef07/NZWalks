using FluentValidation;

namespace NZWalks.API.Validator
{
    public class AddWalkDifficultyRepositoryValidator : AbstractValidator<Model.DTO.AddWalkDifficultyRepository>
    {
        public AddWalkDifficultyRepositoryValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
