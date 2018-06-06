
using System;

namespace OnTheFlyStats
{
    public class PreciseStats : IUpdatable, IStats<decimal>
    {
        /// <summary>
        ///     Last value - mean.
        /// </summary>
        private decimal Delta { get; set; }
        /// <summary>
        ///     Variance * N
        /// </summary>
        private decimal RawVariance { get; set; }

        private decimal LogSum { get; set; }
        /// <summary>
        ///     Number of samples.
        /// </summary>
        public int N { get; private set; }
        /// <summary>
        ///     Sum of all samples.
        /// </summary>
        public decimal Sum { get; private set; }
        /// <summary>
        ///     Minimal observed value across all samples.
        /// </summary>
        public decimal Min { get; private set; } = decimal.MaxValue;
        /// <summary>
        ///     Maximal observed value across all samples.
        /// </summary>
        public decimal Max { get; private set; } = decimal.MinValue;
        /// <summary>
        ///     The range from the minimum to the maximum. Range = Max - Min.
        /// </summary>
        public decimal Range => Max - Min;
        /// <summary>
        /// The average of the min and max of the data. (Max + Min)/2.
        /// </summary>
        public decimal MidRange => (Max + Min) / 2;
        /// <summary>
        ///     Arithmetic mean.
        /// </summary>
        public decimal Average { get; private set; }
        /// <summary>
        ///     Population variance. Assumes knowledge about every sample in population.
        /// </summary>
        public decimal? PopulationVariance => N > 1 ? RawVariance / N : null;
        /// <summary>
        ///     Population variance estimate based on sample.
        /// </summary>
        public decimal? SampleVariance => N > 1 ? RawVariance / (N - 1) : null;
        /// <summary>
        ///     Standard deviation.
        /// </summary>
        public decimal? PopulationStandardDeviation => N > 1 ? Math.Sqrt(PopulationVariance.Value) : null;
        /// <summary>
        ///     Standard deviation estimate.
        /// </summary>
        public decimal SampleStandardDeviation => N > 1 ? Math.Sqrt(SampleVariance) : null;

        public decimal StandardError => N > 1 ? SampleStandardDeviation / Math.Sqrt(N) : null;

        public decimal GeometricAverage => N > 0 ? Math.Exp(LogSum / N) : null;

        public decimal SquareMean { get; private set; }

        public decimal RootMeanSquare => N > 0 ? Math.Sqrt(SquareMean / N) : null;
        /// <summary>
        ///     Call everytime new value is seen.
        /// </summary>
        /// <param name="value">observed value</param>
        public void Update<T>(T value) where T : IConvertible
        {
            var converted = Convert.ToDecimal(value);
            var convertedToDouble = Convert.ToDouble(value);
            ++N;
            Sum += converted;
            Delta = converted - Average;
            Average += Delta / N;
            RawVariance += Delta * (converted - Average);
            LogSum += Convert.ToDecimal(Math.Log(convertedToDouble));
            SquareMean += (converted * converted - SquareMean) / N;
            if (converted < Min) Min = converted;
            if (converted > Max) Max = converted;
        }
        /// <summary>
        /// Sigma distance of actual data from the average. Assumes knowledge about every sample in population and close to normal distribution.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal StandardScore(decimal value)
        {
            return (value - Average) / PopulationStandardDeviation;
        }
        /// <summary>
        /// Distance of the sample mean to the population mean in units of the standard error.
        /// </summary>
        /// <param name="sampleMean"></param>
        /// <returns></returns>
        public decimal Zscore(decimal sampleMean)
        {
            return (sampleMean - Average) / StandardError;
        }

        public override string ToString() => $"Min={Min} Max={Max} μ={Average} N={N}";
    }
}