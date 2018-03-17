using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnTheFlyStats.Test
{
    public class StatsTest
    {
        [Fact]
        public void CanBeCreated()
        {
            var tested = new Stats();
            Assert.NotNull(tested);
        }
        [Fact]
        public void HasProperDefaultValues()
        {
            var tested = new Stats();
            Assert.Equal(double.MinValue, tested.Max);
            Assert.Equal(double.MaxValue, tested.Min);
            Assert.Equal(0, tested.Sum);
            Assert.Equal(0, tested.N);
            Assert.True(double.IsNaN(tested.PopulationStandardDeviation));
            Assert.True(double.IsNaN(tested.PopulationVariance));
            Assert.True(double.IsNaN(tested.SampleStandardDeviation));
            Assert.True(double.IsNaN(tested.SampleVariance));
            Assert.True(double.IsNaN(tested.StandardError));
        }
        [Fact]
        public void NormalizeReturnsNanIfNotEnoughData()
        {
            var tested = new Stats();
            Assert.True(double.IsNaN(tested.StandardScore(10)));
        }
        [Theory]
        [InlineData(new[] { 1, 2, 3, 3.14, 4, 5, 6, 7 }, new[] { 1, 3, 6, 9.14, 13.14, 18.14, 24.14, 31.14 })]
        public void SumAddsAllNumbers(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Sum);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 1.0, 2, 3, 3.14, 4, 4, 4, 7, 7, 15 })]
        public void MaxReturnsMaxValue(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Max);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 1, 1, 1, 1, 1, 1, -1.0, -1.0, -141234, -141234 })]
        public void MinReturnsMinValue(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Min);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10.0 })]
        public void NreturnsCount(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.N);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.5, 0.816496580928, 0.862365931609, 1.03224803221, 1.1207388436, 1.56725159149, 2.24045614775, 44386.3552838, 42371.3715442 })]
        public void PopulationStandardDevReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.PopulationStandardDeviation, 5);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.5, 1, 0.991566666667, 1.33192, 1.50726666667, 2.86565714286, 5.73673571429, 2216417102.3, 1994814585.04 })]
        public void SampleVarianceReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.SampleVariance, 1);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.707106781187, 1, 0.995774405509, 1.15408838483, 1.22770789142, 1.6928251956, 2.39514836999, 47078.83922, 44663.3472216 })]
        public void SampleStandardDevReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.SampleStandardDeviation, 5);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 1, 1.5, 2, 2.285, 2.628, 2.3566666666667, 1.8771428571429, 2.5175, -15690.428888889, -14119.886 })]
        public void AverageReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Average, 5);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.5, 0.577, 0.498, 0.516, 0.501, 0.64, 0.847, 15692.946, 14123.79 })]
        public void StandardErrorOfTheMeanReturnsProperResult(double[] input, double[] expected)
        {
            // http://www.sample-size.net/confidence-interval-mean/
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.StandardError, 2);
            }
        }
        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, 1, 7, 141234, 15 }, new[] { 1, 1.4142135623731, 1.8171205928321 , 2.0833883178232 , 2.3737128342718 , 2.0552128893597, 1.8542273567859, 2.1891719064564, 7.4935302610235, 8.0320600146762 })]
        public void GeometricAverageReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.GeometricAverage, 2);
            }
        }
        
    }
}
