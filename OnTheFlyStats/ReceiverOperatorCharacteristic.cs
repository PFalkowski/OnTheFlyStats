using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using StandardInterfaces;

namespace OnTheFlyStats
{
    /// <summary>
    /// Receiver Operator Characteristics allowing for Confusion Matrix creation https://en.wikipedia.org/wiki/Receiver_operating_characteristic
    /// </summary>
    [Serializable]
    public class ReceiverOperatorCharacteristic : IValueEquatable<ReceiverOperatorCharacteristic>
    {
        public ReceiverOperatorCharacteristic() { }

        public ReceiverOperatorCharacteristic(int truePositives, int falseNegatives, int falsePositives, int trueNegatives)
        {
            TruePositives = truePositives;
            TrueNegatives = trueNegatives;
            FalsePositives = falsePositives;
            FalseNegatives = falseNegatives;
        }

        public ReceiverOperatorCharacteristic(IEnumerable<bool> expected, IEnumerable<bool> predicted)
        {
            var expectedEvaluated = expected.ToList();
            var predictedEvaluated = predicted.ToList();
            if (expectedEvaluated.Count != predictedEvaluated.Count)
                throw new ArgumentOutOfRangeException(nameof(predicted));

            using (var predictedIter = predictedEvaluated.GetEnumerator())
            using (var expectedIter = expectedEvaluated.GetEnumerator())
            {
                while (predictedIter.MoveNext() && expectedIter.MoveNext())
                {
                    Activate(expectedIter.Current, predictedIter.Current);
                }
            }
        }

        [DisplayName("True Positives")]
        [JsonProperty]
        public int TruePositives { get; private set; }

        [DisplayName("True Negatives")]
        [JsonProperty]
        public int TrueNegatives { get; private set; }

        [DisplayName("False Positives")]
        [JsonProperty]
        public int FalsePositives { get; private set; }

        [DisplayName("False Negatives")]
        [JsonProperty]
        public int FalseNegatives { get; private set; }
        
        [JsonIgnore]
        public int ActualPositives => TruePositives + FalseNegatives;
        
        [JsonIgnore]
        public int ActualNegatives => TrueNegatives + FalsePositives;
        
        [JsonIgnore]
        public int All => ActualPositives + ActualNegatives;
        
        [JsonIgnore]
        public int PredictedPositives => TruePositives + FalsePositives;
        
        [JsonIgnore]
        public int PredictedNegatives => TrueNegatives + FalseNegatives;

        /// <summary>
        ///     True Positive Rate (hit rate, recall)(TPR)
        /// </summary>
        [JsonIgnore]
        public double Sensitivity =>
            TruePositives == 0 ? 0 : (double)TruePositives / ActualPositives;

        /// <summary>
        ///     Positive Predictive Value (PPV)
        /// </summary>
        [JsonIgnore]
        public double Precision =>
            TruePositives == 0 ? 0 : (double)TruePositives / PredictedPositives;

        /// <summary>
        ///     True Negative Rate (SPC)
        /// </summary>
        [JsonIgnore]
        public double Specificity =>
            TrueNegatives == 0 ? 0 : (double)TrueNegatives / ActualNegatives;

        /// <summary>
        ///     Average of Sensitivity and Specificity
        /// </summary>
        [JsonIgnore]
        public double Efficiency => (Sensitivity + Specificity) / 2.0;

        /// <summary>
        ///     System Performance (ACC)
        /// </summary>
        [JsonIgnore]
        public double Accuracy =>
            All == 0 ? 0 : (double)(TruePositives + TrueNegatives) / All;

        /// <summary>
        ///     How common is a positive result in data.
        /// </summary>
        [JsonIgnore]
        public double Prevalence =>
            All == 0 ? 0 : (double)ActualPositives / All;
        
        [JsonIgnore]
        public double PositivePredictiveValue =>
            PredictedPositives == 0 ? 1.0 : (double)TruePositives / PredictedPositives;
        
        [JsonIgnore]
        public double NegativePredictiveValue =>
            PredictedNegatives == 0 ? 1.0 : (double)TrueNegatives / PredictedNegatives;
        
        [JsonIgnore]
        public double FalsePositiveRate => (double)FalsePositives / ActualNegatives;
        
        [JsonIgnore]
        public double FalseDiscoveryRate =>
            PredictedPositives == 0 ? 1.0 : (double)FalsePositives / PredictedPositives;

        public bool ValueEquals(ReceiverOperatorCharacteristic other)
        {
            return TruePositives == other.TruePositives &&
                   TrueNegatives == other.TrueNegatives &&
                   FalsePositives == other.FalsePositives &&
                   FalseNegatives == other.FalseNegatives;
        }

        public void Activate(bool real, bool predicted)
        {
            if (real && predicted) ++TruePositives;
            else if (!real && !predicted) ++TrueNegatives;
            else if (!real && predicted) ++FalsePositives;
            else if (real && !predicted) ++FalseNegatives;
        }

        /// <summary>
        ///     Register new observation and classifier prediction.
        /// </summary>
        /// <param name="expected">Value predicted by the model.</param>
        /// <param name="actual">Value from the data.</param>
        /// <param name="threshold"></param>
        public virtual void Activate(double real, double predicted, double threshold = 0.5)
        {
            Activate(real > threshold, predicted > threshold);
        }

        public override string ToString()
        {
            return
                $"{nameof(TruePositives)}: {TruePositives}, {nameof(FalseNegatives)}: {FalseNegatives}, {nameof(FalsePositives)}: {FalsePositives}, {nameof(TrueNegatives)}: {TrueNegatives}, {nameof(Accuracy)}: {Math.Round(Accuracy, 2).ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
