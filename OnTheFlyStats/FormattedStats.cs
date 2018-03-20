using System;
using System.Collections.Generic;
using System.Text;

namespace OnTheFlyStats
{
    public class FormattedStats : Stats, IFormattedStats
    {
        public IStatsFormatter Formatter { get; }
        public FormattedStats(IStatsFormatter formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            Formatter = formatter;
        }
        public string GetFormattedResult() => Formatter.Format(this);
    }
}
