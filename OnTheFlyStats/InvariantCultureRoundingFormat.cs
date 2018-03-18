using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OnTheFlyStats
{
    public class InvariantCultureRoundingFormat : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            return null;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg is double)
            {
                return ((double)arg).ToString("0.##", CultureInfo.InvariantCulture);
            }
            else
            {
                return null;
            }
        }
    }
}
