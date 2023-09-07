using System.Collections.Generic;
using System.Linq;

namespace OnTheFlyStats
{
    public class Sample : Stats
    {
        public string Name { get; set; }
        public List<double> InternalSample { get; set; } = new();

        public Sample() { }

        public Sample(IEnumerable<double> input, string name)
        {
            if (input != null)
            {
                InternalSample = input.ToList();
                InternalSample.ForEach(UpdateInternal);
            }

            Name = name;
        }

        public override void Update(double value)
        {
            InternalSample.Add(value);
            UpdateInternal(value);
        }
    }
}
