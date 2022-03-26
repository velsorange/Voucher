namespace Domain.Validator;

public class ValidationResult
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; }

    private ValidationResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Success() => new(true, string.Empty);
    public static ValidationResult Error(string message) => new(false, message);

    public void AddError(string message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        IsSuccess = false;
        ErrorMessage = string.IsNullOrWhiteSpace(ErrorMessage) ? message : string.Join('\n', ErrorMessage, message);
    }
}