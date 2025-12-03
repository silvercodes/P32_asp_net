namespace _10_service_mw.Services;

public class RequestCounter : IRequestCounter
{
    private int count = 0;
    public int Increment() => ++count;
}
