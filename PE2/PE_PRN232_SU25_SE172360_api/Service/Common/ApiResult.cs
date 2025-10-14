namespace Service.Common;

public class ApiResult<T> where T : class
{
    public string Status { get; set; }
    public string Message { get; set; }
    public T Results { get; set; }
}

public class ErrorResult
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }

    public ErrorResult(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
}

public static class ErrorCodes
{
    public const string InvalidInput = "PR40001";
    public const string AuthenticationError = "PR40101";
    public const string NotAuthorized = "PR40301";
    public const string NotFound = "PR40401";
    public const string InternalServerError = "PR50001";
}