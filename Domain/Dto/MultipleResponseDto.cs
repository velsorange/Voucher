namespace Domain.Dto;

public class MultipleResponseDto<T>  where T : class
{
    public List<T> Objects { get; } = new();
    public List<ErrorDto> Errors { get; } = new();

    public void Ok(T obj)
    {
        Objects.Add(obj);
    }

    public void Error(ErrorDto error)
    {
        Errors.Add(error);
    }
}