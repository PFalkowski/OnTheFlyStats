using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions.Standard;
using Newtonsoft.Json;
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
        [JsonProperty]
        private double Delta { get; set; }

        /// <summary>
        ///     Variance * N
        /// </summary>
        [JsonProperty]
        private double RawVariance { get; set; }
        
        [JsonProperty]
        private double LogSum { get; set; }
        /// <summary>
        ///     Number of samples.
        /// </summary>
        [JsonProperty]
        public int N { get; private set; }

        /// <summary>
        ///     Sum of all samples.
        /// </summary>
        [JsonProperty]
        public double Sum { get; private set; }

        /// <summary>
        ///     Minimal observed value across all samples.
        /// </summary>
        [JsonProperty]
        public double Min { get; private set; } = double.MaxValue;

        /// <summary>
        ///     Maximal observed value across all samples.
        /// </summary>
        [JsonProperty]
        public double Max { get; private set; } = double.MinValue;

        /// <summary>
        ///     Arithmetic mean.
        /// </summary>
        [JsonProperty]
        public double Average { get; private set; }
        
        [JsonProperty]
        public double SquareMean { get; private set; }
        
        /// <summary>
        ///     The range from the minimum to the maximum. Range = Max - Min.
        /// </summary>
        [JsonIgnore]
        public double Range => Max - Min;

        /// <summary>
        /// The average of the min and max of the data. (Max + Min)/2.
        /// </summary>
        [JsonIgnore]
        public double MidRange => (Max + Min) / 2;

        /// <summary>
        ///     Population variance. Assumes knowledge about every sample in population.
        /// </summary>
        [JsonIgnore]
        public double PopulationVariance => N > 1 ? RawVariance / N : double.NaN;

        /// <summary>
        ///     Population variance estimate based on sample.
        /// </summary>
        [JsonIgnore]
        public double SampleVariance => N > 1 ? RawVariance / (N - 1) : double.NaN;

        /// <summary>
        ///     Standard deviation.
        /// </summary>
        [JsonIgnore]
        public double PopulationStandardDeviation => N > 1 ? Math.Sqrt(PopulationVariance) : double.NaN;

        /// <summary>
        ///     Standard deviation estimate.
        /// </summary>
        [JsonIgnore]
        public double SampleStandardDeviation => N > 1 ? Math.Sqrt(SampleVariance) : double.NaN;
        
        [JsonIgnore]
        public double StandardError => N > 1 ? SampleStandardDeviation / Math.Sqrt(N) : double.NaN;
        
        [JsonIgnore]
        public double GeometricAverage => N > 0 ? Math.Exp(LogSum / N) : double.NaN;
        
        [JsonIgnore]
        public double RootMeanSquare => N > 0 ? Math.Sqrt(SquareMean / N) : double.NaN;

        public void Update<TT>(TT value) where TT : IConvertible
        {
            var converted = Convert.ToDouble(value);
            Update(converted);
        }

        /// <summary>
        ///     Call every time new value is seen.
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
            return scaleMin + value * (scaleMax - scaleMin);
        }

        public override string ToString()
        {
            var numericFormat = new InvariantCultureRoundingFormat();
            var stb = new StringBuilder();

            stb.AppendFormat(numericFormat, "μ={0}\t", Average);
            stb.AppendFormat(numericFormat, "Min={0}\t", Min);
            stb.AppendFormat(numericFormat, "Max={0}\t", Max);
            stb.AppendFormat(numericFormat, "∑={0}\t", Sum);
            stb.AppendFormat(numericFormat, "N={0}\t", N);
            stb.AppendFormat(numericFormat, "σ={0}\t", PopulationStandardDeviation);
            stb.AppendFormat(numericFormat, "σ²={0}\t", PopulationVariance);
            stb.AppendFormat(numericFormat, "SEM={0}", StandardError);

            return stb.ToString();
        }

        public string PrettyPrint(string title = "Descriptive statistics calculation result", int lineLength = 60)
        {
            var stb = new StringBuilder();
            string stars = new string('*', lineLength);
            string dashes = new string('-', lineLength - 1);
            stb.AppendLine();
            stb.AppendLine(stars);
            stb.AppendLine($"*{title.CenterText(lineLength)}");
            stb.AppendLine($"*{dashes}");
            stb.AppendLine($"Average                         {Average}");
            stb.AppendLine($"Min                             {Min}");
            stb.AppendLine($"Max                             {Max}");
            stb.AppendLine($"Sum                             {Sum}");
            stb.AppendLine($"N                               {N}");
            stb.AppendLine($"Population standard deviation   {PopulationStandardDeviation}");
            stb.AppendLine($"Population variance             {PopulationVariance}");
            stb.AppendLine($"Standard error of the mean      {StandardError}");
            stb.AppendLine(stars);

            return stb.ToString();
        }
    }
}
