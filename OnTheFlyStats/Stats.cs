using System;

namespace OnTheFlyStats
{
    public class Stats : IUpdatable<double>
    {
        /// <summary>
        ///     Last value - mean.
        /// </summary>
        private double Delta { get; set; }
        /// <summary>
        ///     Variance * N
        /// </summary>
        private double RawVariance { get; set; }

        private double LogSum { get; set; }
        /// <summary>
        ///     Number of samples.
        /// </summary>
        public int N { get; private set; }
        /// <summary>
        ///     Sum of all samples.
        /// </summary>
        public double Sum { get; private set; }
        /// <summary>
        ///     Minimal observed value across all samples.
        /// </summary>
        public double Min { get; private set; } = double.MaxValue;
        /// <summary>
        ///     Maximal observed value across all samples.
        /// </summary>
        public double Max { get; private set; } = double.MinValue;
        /// <summary>
        ///     Mean.
        /// </summary>
        public double Average { get; private set; }
        /// <summary>
        ///     Population variance. Assumes we know every value observable in population.
        /// </summary>
        public double PopulationVariance => N > 1 ? RawVariance / N : double.NaN;
        /// <summary>
        ///     Population variance estimate based on sample.
        /// </summary>
        public double SampleVariance => N > 1 ? RawVariance / (N - 1) : double.NaN;
        /// <summary>
        ///     Standard deviation.
        /// </summary>
        public double PopulationStandardDeviation => N > 1 ? Math.Sqrt(PopulationVariance) : double.NaN;
        /// <summary>
        ///     Standard deviation estimate.
        /// </summary>
        public double SampleStandardDeviation => N > 1 ? Math.Sqrt(SampleVariance) : double.NaN;

        public double StandardError => N > 1 ? SampleStandardDeviation / Math.Sqrt(N) : double.NaN;

        public double GeometricAverage => N > 0 ? Math.Exp(LogSum / N) : double.NaN;

        public double SquareMean { get; private set; }

        public double RootMeanSquare => N > 0 ? Math.Sqrt(SquareMean / N) : double.NaN;
        /// <summary>
        ///     Call everytime new value is seen.
        /// </summary>
        /// <param name="value">observed value</param>
        public void Update(double value)
        {
            ++N;
            Sum += value;
            Delta = value - Average;
            Average += Delta / N;
            RawVariance += Delta * (value - Average);
            LogSum += Math.Log(value);
            SquareMean += (value * value - SquareMean) / N;
            if (value < Min) Min = value;
            if (value > Max) Max = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double StandardScore(double value)
        {
            return (value - Average) / PopulationStandardDeviation;
        }
        /// <summary>
        /// When the population parameters are known
        /// </summary>
        /// <param name="sampleMean"></param>
        /// <returns></returns>
        public double Zscore(double sampleMean)
        {
            return (sampleMean - Average) / StandardError;
        }
        /// <summary>
        ///     For sigma = 3.0 => 99% confidence level for parametric distribution and 88.8889% for any distribution due to
        ///     Chebyshev's inequality. https://en.wikipedia.org/wiki/Confidence_interval
        /// </summary>
        //public double UpperConfidenceBound(double sigma = 1.96)
        //{
        //    return Average + sigma * StandardError;
        //}
        ///// <summary>
        /////     For sigma = 3.0 => 99% confidence level for parametric distribution and 88.8889% for any distribution due to
        /////     Chebyshev's inequality. https://en.wikipedia.org/wiki/Confidence_interval
        ///// </summary>
        //public double LowerConfidenceBound(double sigma = 1.96)
        //{
        //    return Average - sigma * StandardError;
        //}
        ///// <summary>
        /////     https://en.wikipedia.org/wiki/Confidence_interval
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="sigma"></param>
        ///// <returns></returns>
        //public bool InsideConfidenceBounds(double value, double sigma = 1.96)
        //{
        //    return (value >= LowerConfidenceBound(sigma)) && (value <= UpperConfidenceBound(sigma));
        //}
    }
}
