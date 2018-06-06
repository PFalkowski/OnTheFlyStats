using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyStats
{
    public class Normalizer<T> : IUpdatable
    {
        
        public decimal Normalized;

        public void Update<T>(T input) where T : IConvertible
        {
            throw new NotImplementedException();
        }

    }
}
