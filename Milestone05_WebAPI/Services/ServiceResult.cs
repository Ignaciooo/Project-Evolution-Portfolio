namespace Milestone05_WebAPI.Services;

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict
}

public class ServiceResult<T>
{
    public T? Value { get; private set; }
    public ErrorType Error { get; private set; }
    public string Message { get; private set; } = "";

    public bool Success => Error == ErrorType.None;

    public static ServiceResult<T> Ok(T value)
    {
        return new ServiceResult<T> { Value = value };
    }

    public static ServiceResult<T> Fail(ErrorType error, string message)
    {
        return new ServiceResult<T> { Error = error, Message = message };
    }
}