namespace Business.Models;

public class ResponseResult
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}

public class ResponseResult<T> : ResponseResult 
{
    public T? Result { get; set; }
}