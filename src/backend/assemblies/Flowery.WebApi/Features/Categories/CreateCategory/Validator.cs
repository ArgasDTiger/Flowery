using Flowery.WebApi.Shared.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IOptions<TranslationConfiguration> translationSettings)
    {
        RuleFor(x => x.CategoryNames)
            .NotEmpty()
            .WithMessage("At least one category name must be provided.")
            .Must(names => names.Any(n => n.LanguageCode == translationSettings.Value.SlugDefaultLanguage))
            .WithMessage(
                $"Category name for language '{translationSettings.Value.SlugDefaultLanguageString}' must be provided.");

        RuleForEach(x => x.CategoryNames)
            .ChildRules(flower =>
            {
                flower.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name must be provided.");
            });
    }

  
}