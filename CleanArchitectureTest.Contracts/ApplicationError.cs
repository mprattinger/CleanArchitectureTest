using FluentValidation.Results;

namespace CleanArchitectureTest.Contracts;

public class ApplicationError
{
    public string Key { get; set; } = "";

    public string Message { get; set; } = "";

    public ApplicationError(string key, string message)
    {
        Key = key;
        Message = message;
    }


    public ApplicationError(string area, string action, string message)
    {
        Key = $"{area}.{action}";
        Message = message;
    }

    public static ApplicationError ApplicationError_Exception(string area, Exception ex)
    {
        return new ApplicationError(area, "Exception", ex.Message);
    }

    public static ApplicationError ApplicationError_Validation(string area, ValidationResult result)
    {
        var errors = "";
        if (result.Errors.Count() > 1)
        {
            errors = result.Errors.Select(x => x.ToString()).Aggregate((c, n) => $"{c}; {n}");
        }
        else
        {
            errors = result.Errors.FirstOrDefault()?.ToString();
        }

        return new ApplicationError(area, "Validation", errors ?? "");
    }
}
