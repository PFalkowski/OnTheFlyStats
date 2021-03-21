using System;
using System.Collections.Generic;
using System.Text;
using TextFormatting;

namespace OnTheFlyStats
{
    public class Stats : IUpdatable<double>, IStats<double>
    {
        public Stats() { }
        public Stats(IEnumerable<double> input)
        {
            foreach (var item in input)
            {
                Update(item);
            }
        }
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
        ///     The range from the minimum to the maximum. Range = Max - Min.
        /// </summary>
        public double Range => Max - Min;
        /// <summary>
        /// The average of the min and max of the data. (Max + Min)/2.
        /// </summary>
        public double MidRange => (Max + Min) / 2;
        /// <summary>
        ///     Arithmetic mean.
        /// </summary>
        public double Average { get; private set; }
        /// <summary>
        ///     Population variance. Assumes knowledge about every sample in population.
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
        public void Update<TT>(TT value) where TT : IConvertible
        {
            var converted = Convert.ToDouble(value);
            Update(converted);
        }
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
        /// Sigma distance of actual data from the average. Assumes knowledge about every sample in population and close to normal distribution.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double StandardScore(double value)
        {
            return (value - Average) / PopulationStandardDeviation;
        }
        /// <summary>
        /// Distance of the sample mean to the population mean in units of the standard error.
        /// </summary>
        /// <param name="sampleMean"></param>
        /// <returns></returns>
        public double Zscore(double sampleMean)
        {
            return (sampleMean - Average) / StandardError;
        }
        /// <summary>
        /// Scale to this statistic min and max values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double Normalize(double value)
        {
            var scaleMin = Min;
            var scaleMax = Max;
            return scaleMin + value * (scaleMax -  scaleMin);
        }

        public override string ToString()
        {
            var numericFormat = new InvariantCultureRoundingFormat();
            var stb = new StringBuilder();

            stb.AppendFormat(numericFormat, "μ={0}, ", Average);
            stb.AppendFormat(numericFormat, "Min={0}, ", Min);
            stb.AppendFormat(numericFormat, "Max={0}, ", Max);
            stb.AppendFormat(numericFormat, "∑={0}, ", Sum);
            stb.AppendFormat(numericFormat, "N={0}, ", N);
            stb.AppendFormat(numericFormat, "σ={0}, ", PopulationStandardDeviation);
            stb.AppendFormat(numericFormat, "σ²={0}, ", PopulationVariance);
            stb.AppendFormat(numericFormat, "SEM={0}, ", StandardError);

            return stb.ToString();
        }
    }
}
