using System.Net;

namespace Domain.Dto;

public class SingleResponseDto<T> where T : class
{
    public T Data { get; set; }
    public ErrorDto Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}