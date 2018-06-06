using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnTheFlyStats.Test
{
    public class ExtensionsTest
    {
        [Theory]
        [InlineData(new decimal[] { 1.0m, 2, 3, 3.14, 4, 1, -1, 7, -11m, 15m }, -11m, 15m, 42.07m)]
        public void StatsExtensionCalculatesProperly(decimal[] input, decimal min, decimal max, decimal variance)
        {
            var result = input.AsEnumerable().Stats();
            Assert.Equal(input.Length, result.N);
            Assert.Equal(min, result.Min);
            Assert.Equal(max, result.Max);
            Assert.Equal(variance, result.SampleVariance, 2);
        }
    }
}
