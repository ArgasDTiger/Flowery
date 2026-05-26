using Flowery.WebApi.Shared.Extensions;
using FluentValidation;

namespace Flowery.WebApi.Shared.Features;

public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null)
        {
            return next(context);
        }

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            return ValueTask.FromResult<object?>(Results.ValidationProblem(result.ToValidatedDictionary()));
        }

        return next(context);
    }
}