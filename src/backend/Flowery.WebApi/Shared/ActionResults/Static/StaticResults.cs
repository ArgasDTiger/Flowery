namespace Flowery.WebApi.Shared.ActionResults.Static;

public static class StaticResults
{
    public static readonly NotFound NotFound = new();
    public static readonly Success Success = new();
    public static readonly Error Error = new("Unexpected error occured.");
}