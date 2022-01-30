using OnTheFlyStats.Test.Mocks;
using System.Globalization;
using Xunit;

namespace OnTheFlyStats.Test
{
    public class OneLineStatsFormatterTest
    {
        [Theory]
        [ClassData(typeof(StatsMock1))]
        public void DefaultCtorSetsInvariantCultureFormatter(Stats stats)
        {

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("PL-pl");
                var tested = new OneLineStatsFormatter();

                var received = tested.Format(stats);

                Assert.Equal("μ=-14119.89, σ=42371.37, ∑=-141198.86, SEM=14123.79, Min=-141234, Max=15, N=10", received);
            }
            finally
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentCulture;
            }
        }

        [Theory]
        [ClassData(typeof(StatsMock1))]
        public void FormatsProperlyWithoutFormatter(Stats stats)
        {
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                var tested = new OneLineStatsFormatter(null);

                var received = tested.Format(stats);

                Assert.Contains("μ=-14119", received);
                Assert.Contains("σ=42371", received);
                Assert.Contains("∑=-141198", received);
            }
            finally
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentCulture;
            }
        }
    }
}
