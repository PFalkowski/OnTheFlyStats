namespace OnTheFlyStats
{
    public interface IFormatter<T>
    {
        string Format(T input);
    }
}
