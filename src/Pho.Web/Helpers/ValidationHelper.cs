using Microsoft.AspNetCore.Mvc;

namespace Pho.Web.Helpers;

public static class ValidationHelper
{
    public static ValidationProblemDetails SingleValidationError(string parameterName, string validationError)
    {
        return new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            [parameterName] = new[] {validationError},
        });
    }
}
