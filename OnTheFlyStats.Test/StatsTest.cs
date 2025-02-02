﻿using System;
using System.Globalization;
using System.Linq;
using Xunit;
using Extensions;
using Extensions.Standard;
using Newtonsoft.Json;

namespace OnTheFlyStats.Test
{
    /// <summary>
    /// https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php
    /// </summary>
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
            Assert.True(double.IsNaN(tested.Max));
            Assert.True(double.IsNaN(tested.Min));
            Assert.Equal(0, tested.Sum);
            Assert.Equal(0, tested.N);
            Assert.True(double.IsNaN(tested.PopulationStandardDeviation));
            Assert.True(double.IsNaN(tested.PopulationVariance));
            Assert.True(double.IsNaN(tested.StandardDeviation));
            Assert.True(double.IsNaN(tested.Variance));
            Assert.True(double.IsNaN(tested.StandardError));
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3, 4.0 })]
        public void DisplaysValidInfoInToString(double[] input)
        {
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                var tested = new Stats();
                for (int i = 0; i < input.Length; ++i)
                {
                    tested.Update(input[i]);
                }
                var actual = tested;
                Assert.Contains($"Mean={Math.Round(actual.Mean, 1)}", actual.ToString());
                Assert.Contains($"n={actual.N}", actual.ToString());
                Assert.Contains($"Min={Math.Round(actual.Min, 1)}", actual.ToString());
                Assert.Contains($"Max={Math.Round(actual.Max, 1)}", actual.ToString());
            }
            finally
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            }
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
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void NreturnsCount2(int repetitions)
        {
            var tested = new Stats();
            for (int i = 0; i < repetitions; ++i)
            {
                tested.Update(i);
            }
            Assert.Equal(repetitions, tested.N);
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 4, 5 }, new[] { double.NaN, 0.25, 0.66666667, 1.25, 2 })]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 },
            new[] { double.NaN, 0.25, 0.66666667, 0.743675, 1.065536, 1.2560556, 2.4562776, 5.0196438, 1970148535.37, 1795333126.53 })]
        public void PopulationVarianceReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.PopulationVariance, 1);
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
        [InlineData(new[] { 1.0, 2, 3, 4, 5 }, new[] { double.NaN, 0.5, 1, 1.6666667, 2.5 })]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.5, 1, 0.991566666667, 1.33192, 1.50726666667, 2.86565714286, 5.73673571429, 2216417102.3, 1994814585.04 })]
        public void SampleVarianceReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Variance, 1);
            }
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { double.NaN, 0.70711, 1, 0.995774405509, 1.15408838483, 1.22770789142, 1.6928251956, 2.39514836999, 47078.83922, 44663.3472216 })]
        public void SampleStandardDevReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.StandardDeviation, 5);
            }
        }

        [Theory]
        [InlineData(new[] { 1.0 }, 1)]
        [InlineData(new[] { 1.0, 2 }, 1.581138830)]
        [InlineData(new[] { 1.0, 2, 3 }, 2.160246900)]
        [InlineData(new[] { 1.0, 2, 3, 4 }, 2.738612787)]
        [InlineData(new[] { 1.0, 2, 3, 4, 5 }, 3.316624790)]
        public void RootMeanSquare_ShouldReturnCorrectRootMeanSquare(double[] input, double expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
            }
            Assert.Equal(expected, tested.RootMeanSquare, 5);
        }

        [Theory]
        [InlineData(new[] { 1.0 }, 1)]
        [InlineData(new[] { 1.0, 2 }, 1.414213562373)]
        [InlineData(new[] { 1.0, 2, 3 }, 1.817120592832)]
        [InlineData(new[] { 1.0, 2, 3, 4 }, 2.213363839401)]
        [InlineData(new[] { 1.0, 2, 3, 4, 5 }, 2.605171084697)]
        public void GeometricAverage_ShouldReturnCorrectGeometricAverage(double[] input, double expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
            }
            Assert.Equal(expected, tested.GeometricAverage, 5);
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 1, 1.5, 2, 2.285, 2.628, 2.3566666666667, 1.8771428571429, 2.5175, -15690.428888889, -14119.886 })]
        public void AverageReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Mean, 5);
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
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, 1, 7, 141234, 15 },
            new[] { 1, 1.4142135623731, 1.8171205928321, 2.0833883178232, 2.3737128342718, 2.0552128893597, 1.8542273567859, 2.1891719064564, 7.4935302610235, 8.0320600146762 })]
        [InlineData(new[] { 2.0, 2, 5, 7 }, new[] { 2, 2, 2.7144176165949, 3.4397906282504 })]
        public void GeometricAverageReturnsProperResult(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.GeometricAverage, 2);
            }
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, -12 }, //, 4, 1, 1, 7, 141234, 15 
            new[] { 1.0, 1.0, 1.0, 2.0, 0.0 },
            new[] { double.NaN, -1.0, -1.22475, -0.33049, 0.09921 })]
        public void ZscoreReturnsProperResult(double[] populationList, double[] sampleList, double[] expectedList)
        {
            var tested = new Stats();
            for (int i = 0; i < populationList.Length; ++i)
            {
                tested.Update(populationList[i]);
                var actual = tested.StandardScore(sampleList[i]);
                var expected = expectedList[i];
                Assert.Equal(expected, actual, 2);
            }
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, -12 }, //, 4, 1, 1, 7, 141234, 15 
            new[] { 1.0, 1.0, 1.0, 2.0, 0.0 },
            new[] { double.NaN, -1.0, -1.22475, -0.33049, 0.09921 })]
        public void StandardScoreReturnsProperResult(double[] populationList, double[] sampleList, double[] expectedList)
        {
            var tested = new Stats();
            for (int i = 0; i < populationList.Length; ++i)
            {
                tested.Update(populationList[i]);
                var actual = tested.StandardScore(sampleList[i]);
                var expected = expectedList[i];
                Assert.Equal(expected, actual, 2);
            }
        }

        [Theory]
        [InlineData(new[] { 1.0, 2, 3, 3.14, 4, 1, -1, 7, -141234, 15 }, new[] { 0.0, 1, 2, 2.14, 3, 3, 5, 8, 141241, 141249 })]
        public void RangeReturnsProperResult(double[] input, double[] expected)
        {
            // http://www.sample-size.net/confidence-interval-mean/
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                tested.Update(input[i]);
                Assert.Equal(expected[i], tested.Range);
            }
        }

        [Theory]
        [InlineData(new double[] { 1.0, 2, 3, 3.14159265359, 4, 1, -1, 7, 141234, 17 },
            new double[] { 1.0, 3, 6, 9.14159265359, 13.14159265359, 14.14159265359, 13.14159265359, 20.14159265359, 141254.14159265359, 141271.14159265359 })]
        public void SumHasRequiredPrecision(double[] input, double[] expected)
        {
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                var currentInput = input[i];
                var currentExpected = expected[i];
                tested.Update(currentInput);
                Assert.Equal(currentExpected, tested.Sum);
            }
        }

        [Fact]
        public void GenericUpdateWorks()
        {
            decimal[] input = new decimal[] { 1.0m, 2m, 3m, 3.14159265359m, 4m, 1m, -1m, 7m, 141234m, 17m };
            decimal[] expected = new decimal[] { 1.0m, 3, 6, 9.14159265359m, 13.14159265359m, 14.14159265359m, 13.14159265359m, 20.14159265359m, 141254.14159265359m, 141271.14159265359m };
            var tested = new Stats();
            for (int i = 0; i < input.Length; ++i)
            {
                var currentInput = input[i];
                var currentExpected = expected[i];
                tested.Update(currentInput);
                Assert.Equal(currentExpected, (decimal)tested.Sum, 5);
            }
        }

        [Fact]
        public void ScaleScalesProperly()
        {
            var input = new double[] { 1, 2, 3, 4, 5 };
            var tested = new Stats(input);

            var testValue = 10.0;
            var expected = 41;
            var actual = tested.Normalize(testValue);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSerializeAndDeserialize()
        {
            // Arrange
            var input = new double[] { 1, 2, 3, 4, 5 };
            var tested = new Stats(input);

            // Act
            var serialized = JsonConvert.SerializeObject(tested);
            var deserialized = JsonConvert.DeserializeObject<Stats>(serialized);

            // Assert
            Assert.Equal(tested.ToString(), deserialized.ToString());
        }

        [Fact]
        public void PrettyPrintPrintsPretty()
        {
            // Arrange
            var input = new double[] { 1, 2, 3, 4, 5 };
            var tested = new Stats(input);

            // Act
            var prettyPrinted = tested.PrettyPrint();
            const string expected = @"
************************************************************
*         Descriptive statistics calculation result         
*-----------------------------------------------------------
Mean                            3
Min                             1
Max                             5
Sum                             15
Count                           5
Standard deviation              1.4142135623730951
Variance                        2
Standard error                  0.7071067811865476
************************************************************
";

            // Assert
            Assert.Equal(expected, prettyPrinted);
        }

        [Fact]
        public void Reset_ShouldSetAllValuesToInitialState()
        {
            // Arrange
            var stats = new Stats(new[] { 1.0, 2.0, 3.0, 4.0, 5.0 });

            // Act
            stats.Reset();

            // Assert
            Assert.Equal(0, stats.Count);
            Assert.Equal(0, stats.Sum);
            Assert.Equal(0, stats.LogSum);
            Assert.Equal(0, stats.Mean);
            Assert.Equal(0, stats.SquareMean);
            Assert.True(double.IsNaN(stats.Min));
            Assert.True(double.IsNaN(stats.Max));
            Assert.True(double.IsNaN(stats.Variance));
            Assert.True(double.IsNaN(stats.StandardDeviation));
            Assert.True(double.IsNaN(stats.PopulationVariance));
            Assert.True(double.IsNaN(stats.PopulationStandardDeviation));
            Assert.True(double.IsNaN(stats.GeometricAverage));
            Assert.True(double.IsNaN(stats.RootMeanSquare));
            Assert.True(double.IsNaN(stats.StandardError));
        }

        [Fact]
        public void Reset_ShouldAllowNewUpdatesAfterReset()
        {
            // Arrange
            var stats = new Stats(new[] { 1.0, 2.0, 3.0 });

            // Act
            stats.Reset();
            stats.Update(10.0);
            stats.Update(20.0);

            // Assert
            Assert.Equal(2, stats.Count);
            Assert.Equal(30.0, stats.Sum);
            Assert.Equal(15.0, stats.Mean);
            Assert.Equal(10.0, stats.Min);
            Assert.Equal(20.0, stats.Max);
        }
    }
}
