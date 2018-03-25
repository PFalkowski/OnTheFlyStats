using OnTheFlyStats.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnTheFlyStats.Test
{
    public class FormattedStatsTest
    {
        [Fact]
        public void DefaultCtorHasProperDefaults()
        {
            var tested = new FormattedStats();

            Assert.IsType<OneLineStatsFormatter>(tested.Formatter);
        }
        [Fact]
        public void FormatsProperly()
        {
            var formatter = new OneLineStatsFormatter(new InvariantCultureRoundingFormat());
            var tested = new FormattedStats(formatter);
            tested.Update(1.0);
            tested.Update(2);
            tested.Update(3);
            tested.Update(3.14);
            tested.Update(4);
            tested.Update(1);
            tested.Update(-1);
            tested.Update(7);
            tested.Update(-141234);
            tested.Update(15);

            var received = tested.GetFormattedResult();

            Assert.Equal("μ=-14119.89, σ=42371.37, ∑=-141198.86, SEM=14123.79, Min=-141234, Max=15, N=10", received);
        }
    }
}
