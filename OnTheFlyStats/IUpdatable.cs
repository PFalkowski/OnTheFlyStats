using System;

namespace OnTheFlyStats
{
    public interface IUpdatable
    {
        void Update<T>(T input) where T : IConvertible;
    }
}
