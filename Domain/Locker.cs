namespace Domain;

public class Locker
{
    public SemaphoreSlim SemaphoreSlim { get; } = new(1);
}