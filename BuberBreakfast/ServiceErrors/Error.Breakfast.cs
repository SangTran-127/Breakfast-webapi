namespace BuberBreakfast.ServiceErrors;

using ErrorOr;

public static class Errors
{
    public static class Breakfast
    {
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast not found"
        );
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Breakfast name must be at least {Models.Breakfast.MIN_NAME_LENGTH} characters and maximum length {Models.Breakfast.MAX_NAME_LENGTH}"
        );
    }
}