using Flowery.WebApi.Shared.Enums;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Settings;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly IValidator<Request> _validator;
    private readonly TranslationSettings _translationSettings;

    public Handler(IQuery query, IValidator<Request> validator, IOptions<TranslationSettings> translationSettings)
    {
        _query = query;
        _validator = validator;
        _translationSettings = translationSettings.Value;
    }

    public async Task<OneOf<int, List<ValidationFailure>>> CreateFlower(Request request,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors;
        }

        DatabaseModel dbModel = DatabaseModel.FromRequest(request);
        LanguageCode defaultLanguageName = _translationSettings.SlugDefaultLanguage;
        string flowerName = dbModel.FlowerNames.Where(fn => fn.LanguageCode == defaultLanguageName)
                                .Select(fn => fn.Name).FirstOrDefault() ??
                            dbModel.FlowerNames[0].Name;

        string slug = flowerName.GenerateSlug();

        // TODO: verify if slug exists
        dbModel.Slug = slug;

        return await _query.CreateFlower(dbModel, cancellationToken);
    }
}