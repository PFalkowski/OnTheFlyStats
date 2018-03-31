using System;
using TextFormatting;

namespace OnTheFlyStats
{
    public class FormattedStats : Stats, IFormattedStats
    {
        public IFormatter<Stats> Formatter { get; set; }

        public FormattedStats()
        {
            Formatter = new OneLineStatsFormatter(new InvariantCultureRoundingFormat());
        }

        public FormattedStats(IFormatter<Stats> formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            Formatter = formatter;
        }

        public string GetFormattedResult() => Formatter.Format(this);
    }
}
