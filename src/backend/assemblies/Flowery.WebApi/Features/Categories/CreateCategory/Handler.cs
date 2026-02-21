using Flowery.Shared.ActionResults;
using Flowery.Shared.Exceptions;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly TranslationConfiguration _translationSettings;

    public Handler(IQuery query, IOptions<TranslationConfiguration> translationSettings)
    {
        _query = query;
        _translationSettings = translationSettings.Value;
    }

    public async Task<OneOf<string, Error>> CreateCategory(Request request, CancellationToken cancellationToken)
    {
        string categoryName = request.CategoryNames.AsValueEnumerable()
                                  .Where(cn => cn.LanguageCode == _translationSettings.SlugDefaultLanguage)
                                  .Select(cn => cn.Name)
                                  .FirstOrDefault() ??
                              throw new DefaultLanguageTranslationMissingException(_translationSettings
                                  .SlugDefaultLanguage);
        bool categoryExists = await _query.CategoryExists(categoryName, cancellationToken);
        if (categoryExists)
        {
            return new Error($"Category with name {categoryName} already exists.");
        }

        string slug = categoryName.GenerateSlug(_translationSettings.SlugDefaultLanguage, addPrefix: false);

        var dbModel = new DatabaseModel(Slug: slug, CategoryNames: request.CategoryNames);
        await _query.CreateCategory(dbModel, cancellationToken);
        return slug;
    }
}