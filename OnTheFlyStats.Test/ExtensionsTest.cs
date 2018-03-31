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
        [InlineData(new double[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -11, 15 }, -11, 15, 42.07)]
        public void StatsExtensionCalculatesProperly(double[] input, double min, double max, double variance)
        {
            var result = input.AsEnumerable().Stats();
            Assert.Equal(input.Length, result.N);
            Assert.Equal(min, result.Min);
            Assert.Equal(max, result.Max);
            Assert.Equal(variance, result.SampleVariance, 2);
        }
    }
}
