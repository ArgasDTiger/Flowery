using Flowery.WebApi.Features.Categories.CreateCategory;
using Flowery.WebApi.Shared.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public sealed record Request(ImmutableArray<CategoryNameRequest> CategoryNames);

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
            .ChildRules(category =>
            {
                category.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name must be provided.");
            });
    }
}