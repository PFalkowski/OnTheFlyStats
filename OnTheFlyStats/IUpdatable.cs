namespace OnTheFlyStats
{
    public interface IUpdatable<in T>
    {
        void Update(T input);
    }
}
