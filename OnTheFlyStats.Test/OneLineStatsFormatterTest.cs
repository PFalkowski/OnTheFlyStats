﻿using NSubstitute;
using OnTheFlyStats.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                Assert.Equal("μ=-14119.886, σ=42371.3715441566, ∑=-141198.86, SEM=14123.7905147189, Min=-141234, Max=15, N=10", received);
            }
            finally
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentCulture;
            }
        }
    }
}
