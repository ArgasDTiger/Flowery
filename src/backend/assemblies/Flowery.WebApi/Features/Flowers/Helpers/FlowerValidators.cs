using FluentValidation;

namespace Flowery.WebApi.Features.Flowers.Helpers;

public static class FlowerValidators
{
    private const int MaxImageSize = 5 * 1024 * 1024;

    public static IRuleBuilderOptions<T, IFormFile> MustBeValidFlowerImage<T>(
        this IRuleBuilderOptions<T, IFormFile> rule)
    {
        return rule.Must(image => image.Length <= MaxImageSize)
            .WithMessage($"Image size must not exceed {MaxImageSize} bytes.")
            .Must(image => image.ContentType is "image/jpeg" or "image/png" or "image/webp")
            .WithMessage("Primary image must be a JPEG, PNG or WebP file.");
    }
}