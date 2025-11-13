namespace Service.Common;

public class ServiceResult<T> where T : class
{
    public string Status { get; set; }
    public string Message { get; set; }
    public T Results { get; set; }
}

public class ErrorResult
{
    public ErrorResult(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }

    public string ErrorCode { get; set; }
    public string Message { get; set; }
}