using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;
using Flowery.Shared.Exceptions;
using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly TranslationConfiguration _translationConfiguration;

    public Handler(IQuery query, IOptions<TranslationConfiguration> translationSettings)
    {
        _query = query;
        _translationConfiguration = translationSettings.Value;
    }

    public async Task<OneOf<Success, NotFound>> UpdateCategory(string slug, Request request,
        CancellationToken cancellationToken)
    {
        var category = await _query.GetCategoryBySlug(slug, cancellationToken);
        if (category is null) return StaticResults.NotFound;

        string categoryName = request.CategoryNames
                                  .AsValueEnumerable()
                                  .Where(cn => cn.LanguageCode == _translationConfiguration.SlugDefaultLanguage)
                                  .Select(cn => cn.Name)
                                  .FirstOrDefault() ??
                              throw new DefaultLanguageTranslationMissingException(
                                  _translationConfiguration.SlugDefaultLanguage);

        bool nameChanged = !string.Equals(categoryName, category.Name, StringComparison.OrdinalIgnoreCase);
        var dbModel = new DatabaseModel(
            Id: category.Id,
            OldSlug: slug,
            NewSlug: nameChanged ? categoryName.GenerateSlug(_translationConfiguration.SlugDefaultLanguage, addPrefix: false) : null,
            CategoryNames: request.CategoryNames,
            NameChanged: nameChanged);

        return await _query.UpdateCategory(dbModel, cancellationToken);
    }
}