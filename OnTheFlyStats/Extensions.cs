using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyStats
{
    public static class Extensions
    {
        public static Stats Stats<T>(this IEnumerable<T> input) where T : IConvertible
        {
            var result = new Stats();
            foreach (var item in input)
            {
                var converted = Convert.ToDouble(item);
                result.Update(converted);
            }
            return result;
        }
    }
}
