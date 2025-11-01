using FluentValidation;
using FluentValidation.Results;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly IValidator<Request> _validator;

    public Handler(IQuery query, IValidator<Request> validator)
    {
        _query = query;
        _validator = validator;
    }

    public async Task<OneOf<ImmutableArray<Response>, IReadOnlyList<ValidationFailure>>> GetFlowers(Request request,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.Errors;
        }

        return await _query.GetFlowers(request, cancellationToken);
    }
}