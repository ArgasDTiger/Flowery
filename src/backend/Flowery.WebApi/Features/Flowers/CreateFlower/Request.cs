using Flowery.WebApi.Shared.Enums;
using FluentValidation;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Request
{
    public decimal Price { get; init; }
    public IReadOnlyList<FlowerNameRequest> FlowerNames { get; init; } = null!;
    public string Description { get; init; } = string.Empty;
}

public sealed class FlowerNameRequest
{
    public LanguageCode LanguageCode { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
        
        RuleFor(x => x.FlowerNames)
            .NotEmpty()
            .WithMessage("At least one flower name must be provided.");

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