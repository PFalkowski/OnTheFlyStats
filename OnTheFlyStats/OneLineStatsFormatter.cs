using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OnTheFlyStats
{
    public class OneLineStatsFormatter : IStatsFormatter
    {
        public IFormatProvider NumericFormat { get; set; }

        public OneLineStatsFormatter()
        {
            NumericFormat = new InvariantCultureRoundingFormat();
        }

        public OneLineStatsFormatter(IFormatProvider numericFormat)
        {
            NumericFormat = numericFormat;
        }

        public string Format(Stats stats)
        {
            var stb = new StringBuilder();

            stb.AppendFormat(NumericFormat, "μ={0}, ", stats.Average);
            stb.AppendFormat(NumericFormat, "σ={0}, ", stats.PopulationStandardDeviation);
            //stb.AppendFormat(NumericFormat, "σ²={0}, ", stats.PopulationVariance);
            stb.AppendFormat(NumericFormat, "∑={0}, ", stats.Sum);
            stb.AppendFormat(NumericFormat, "SEM={0}, ", stats.StandardError);
            stb.AppendFormat(NumericFormat, "Min={0}, ", stats.Min);
            stb.AppendFormat(NumericFormat, "Max={0}, ", stats.Max);
            stb.AppendFormat(NumericFormat, "N={0}", stats.N);

            return stb.ToString();
        }
    }
}
