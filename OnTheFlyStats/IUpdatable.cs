using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyStats
{
    public interface IUpdatable<T>
    {
        void Update(T input);
    }
}
