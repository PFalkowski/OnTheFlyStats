using Extensions.Standard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                UpdateInternal(item);
            }
        }
        
        /// <summary>
        ///     Variance * Count
        /// </summary>
        [JsonProperty]
        private double RawVariance { get; set; }

        [JsonProperty]
        public double LogSum { get; private set; }

        /// <summary>
        ///     Number of samples.
        /// </summary>
        [JsonProperty]
        public int Count { get; private set; }

        /// <summary>
        ///     Number of samples.
        /// </summary>
        [JsonProperty]
        public int N => Count;

        /// <summary>
        ///     Sum of all samples.
        /// </summary>
        [JsonProperty]
        public double Sum { get; private set; }

        /// <summary>
        ///     Minimal observed value across all samples.
        /// </summary>
        [JsonProperty]
        public double Min { get; private set; } = double.NaN;

        /// <summary>
        ///     Maximal observed value across all samples.
        /// </summary>
        [JsonProperty]
        public double Max { get; private set; } = double.NaN;

        /// <summary>
        ///     Arithmetic mean.
        /// </summary>
        [JsonProperty]
        [Obsolete("Use Mean")]
        public double Average => Mean;

        /// <summary>
        ///     Arithmetic mean.
        /// </summary>
        [JsonProperty]
        public double Mean { get; private set; }

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
        public double PopulationVariance => Count > 1 ? RawVariance / Count : double.NaN;

        /// <summary>
        ///     Sample variance, or to be exact, population variance estimate based on sample.
        /// </summary>
        [JsonIgnore]
        public double Variance => Count > 1 ? RawVariance / (Count - 1) : double.NaN;

        [Obsolete("Use Variance")]
        [JsonIgnore]
        public double SampleVariance => Variance;

        /// <summary>
        ///     Standard deviation.
        /// </summary>
        [JsonIgnore]
        public double PopulationStandardDeviation => Count > 1 ? Math.Sqrt(PopulationVariance) : double.NaN;

        /// <summary>
        ///     Standard deviation estimate.
        /// </summary>
        [JsonIgnore]
        public double StandardDeviation => Count > 1 ? Math.Sqrt(Variance) : double.NaN;

        [Obsolete]
        [JsonIgnore]
        public double SampleStandardDeviation => Variance;

        [JsonIgnore]
        public double StandardError => Count > 1 ? StandardDeviation / Math.Sqrt(Count) : double.NaN;
        
        [JsonIgnore]
        public double GeometricAverage => Count > 0 ? Math.Exp(LogSum / Count) : double.NaN;
        
        [JsonIgnore]
        public double RootMeanSquare => Count > 0 ? Math.Sqrt(SquareMean) : double.NaN;

        public void Update<TT>(TT value) where TT : IConvertible
        {
            var converted = Convert.ToDouble(value);
            Update(converted);
        }

        public virtual void Update(double value)
        {
            UpdateInternal(value);
        }

        /// <summary>
        ///     Add observation to this statistics.
        ///     Call every time new value is seen.
        /// </summary>
        /// <param name="value">observed value</param>
        protected void UpdateInternal(double value)
        {
            ++Count;
            Sum += value;
            var delta = value - Mean;
            Mean += delta / Count;
            RawVariance += delta * (value - Mean);
            LogSum += Math.Log(value);
            SquareMean += (value * value - SquareMean) / Count;
            if (value < Min || double.IsNaN(Min)) Min = value;
            if (value > Max || double.IsNaN(Max)) Max = value;
        }

        /// <summary>
        /// Distance of the <paramref name="sampleMean"/> to the population mean in units of the standard error.
        /// </summary>
        /// <param name="sampleMean"></param>
        /// <returns></returns>
        [Obsolete($"Use {nameof(StandardScore)}(double sampleMean, int sampleSize)")]
        public double Zscore(double sampleMean)
        {
            return (sampleMean - Mean) / StandardError;
        }

        /// <summary>
        /// Standard score, most commonly called Z-score. Z-score of a single data point <paramref name="x"/>.
        /// The difference of the raw data score minus the population mean, divided by the population standard deviation.
        /// Sigma (standard deviation) distance of single data point from the population average.
        /// Assumes normal distribution and knowledge about whole population.
        /// https://www.calculatorsoup.com/calculators/statistics/z-score-calculator.php
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double StandardScore(double x)
        {
            return (x - Mean) / PopulationStandardDeviation;
        }

        /// <summary>
        /// Z-score of a sample with known population standard deviation.
        /// the sample mean minus the population mean,
        /// divided by the Standard Error of the Mean for a Population which is the population standard deviation divided by the square root of the sample size
        /// Standard score, most commonly called Z-score. 
        /// Sigma (standard deviation) distance of single data point from the population average.
        /// Assumes normal distribution and knowledge about whole population.
        /// https://www.calculatorsoup.com/calculators/statistics/z-score-calculator.php
        /// </summary>
        /// <param name="sampleMean"></param>
        /// <param name="sampleSize"></param>
        /// <returns></returns>
        public double StandardScore(double sampleMean, int sampleSize)
        {
            return (sampleMean - Mean) / (PopulationStandardDeviation / Math.Sqrt(sampleSize));
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

        public void Reset()
        {
            Count = 0;
            Sum = 0;
            LogSum = 0;
            RawVariance = 0;
            Mean = 0;
            SquareMean = 0;
            Min = double.NaN;
            Max = double.NaN;
        }

        public override string ToString()
        {
            var numericFormat = new InvariantCultureRoundingFormat();
            var stb = new StringBuilder();

            stb.AppendFormat(numericFormat, "Mean={0}\t", Mean);
            stb.AppendFormat(numericFormat, "Min={0}\t", Min);
            stb.AppendFormat(numericFormat, "Max={0}\t", Max);
            stb.AppendFormat(numericFormat, "Sum={0}\t", Sum);
            stb.AppendFormat(numericFormat, "n={0}\t", Count);
            stb.AppendFormat(numericFormat, "Sd={0}\t", StandardDeviation);
            stb.AppendFormat(numericFormat, "Sd²={0}\t", Variance);
            stb.AppendFormat(numericFormat, "SEM={0}", StandardError);

            return stb.ToString();
        }

        public string PrettyPrint(string title = "Descriptive statistics calculation result",
            int lineLength = 60, CultureInfo culture = null)
        {
            var stb = new StringBuilder();
            var stars = new string('*', lineLength);
            var dashes = new string('-', lineLength - 1);
            stb.AppendLine();
            stb.AppendLine(stars);
            stb.AppendLine($"*{title.CenterText(lineLength)}");
            stb.AppendLine($"*{dashes}");
            stb.AppendLine($"Mean                            {Mean.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Min                             {Min.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Max                             {Max.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Sum                             {Sum.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Count                           {Count.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Standard deviation              {PopulationStandardDeviation.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Variance                        {PopulationVariance.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine($"Standard error                  {StandardError.ToString(culture ?? CultureInfo.InvariantCulture)}");
            stb.AppendLine(stars);

            return stb.ToString();
        }
    }
}
