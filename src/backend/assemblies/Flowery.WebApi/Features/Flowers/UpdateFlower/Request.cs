using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed record Request(decimal Price, ImmutableArray<FlowerNameRequest> FlowerNames, string Description);

public sealed record FlowerNameRequest(LanguageCode LanguageCode, string Name);

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator(IOptions<TranslationConfiguration> translationSettings)
    {
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.FlowerNames)
            .NotEmpty()
            .WithMessage("At least one flower name must be provided.")
            .Must(names => names.Any(n => n.LanguageCode == translationSettings.Value.SlugDefaultLanguage))
            .WithMessage($"Flower name for language '{translationSettings.Value.SlugDefaultLanguageString}' must be provided.");

        RuleForEach(x => x.FlowerNames)
            .ChildRules(flower =>
            {
                flower.RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name must be provided.");
            });

        RuleForEach(x => x.Description)
            .NotEmpty()
            .WithMessage("Description must be provided.");
    }
}