using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyStats
{
    public interface IStatsFormatter
    {
        string Format(Stats stats);
    }
}
