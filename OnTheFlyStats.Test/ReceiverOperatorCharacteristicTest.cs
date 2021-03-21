using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnTheFlyStats.Test
{
    public class ReceiverOperatorCharacteristicTest
    {
        private const int TruePositives = 1;
        private const int FalsePositives = 2;
        private const int FalseNegatives = 3;
        private const int TrueNegatives = 4;

        private readonly IReadOnlyList<bool> _resultsList = new ReadOnlyCollection<bool>(new List<bool>{
            true,
            true, true,
            false, false, false,
            false, false, false, false
        });
        private readonly IReadOnlyList<bool> _expectedList = new ReadOnlyCollection<bool>(new List<bool>{
            true,
            false, false,
            true, true, true,
            false, false, false, false
        });

        [Fact]
        public void ConstructorSetsParametersCorrectly()
        {
            // Act
            var tested = new ReceiverOperatorCharacteristic(TruePositives, FalseNegatives, FalsePositives, TrueNegatives);

            // Assert
            Assert.Equal(TruePositives, tested.TruePositives);
            Assert.Equal(FalseNegatives, tested.FalseNegatives);
            Assert.Equal(FalsePositives, tested.FalsePositives);
            Assert.Equal(TrueNegatives, tested.TrueNegatives);
        }

        [Fact]
        public void Constructor2SetsParametersCorrectly()
        {
            // Act
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(TruePositives, tested.TruePositives);
            Assert.Equal(FalseNegatives, tested.FalseNegatives);
            Assert.Equal(FalsePositives, tested.FalsePositives);
            Assert.Equal(TrueNegatives, tested.TrueNegatives);
        }

        [Fact]
        public void Constructor2ThrowsOutOfRangeExceptionWhenDifferentSizes()
        {
            // Arrange

            var resultsList = new List<bool>
            {
                true,
            };
            var expectedList = new List<bool>
            {
                true,
                false
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReceiverOperatorCharacteristic(expectedList, resultsList));
        }
        
        [Fact]
        public void ActualPositivesIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(_expectedList.Count(x => x), tested.ActualPositives);
        }

        [Fact]
        public void ActualNegativesIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(_expectedList.Count(x => !x), tested.ActualNegatives);
        }
        
        [Fact]
        public void AllIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(_expectedList.Count, tested.All);
        }

        [Fact]
        public void PredictedPositivesIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(_resultsList.Count(x => x), tested.PredictedPositives);
        }

        [Fact]
        public void PredictedNegativesIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(_resultsList.Count(x => !x), tested.PredictedNegatives);
        }
        
        [Fact]
        public void AccuracyIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(.5, tested.Accuracy);
        }
        
        [Fact]
        public void PrecisionIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(.33333333333333, tested.Precision, 10);
        }

        [Fact]
        public void SensitivityIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(.25, tested.Sensitivity, 10);
        }

        [Fact]
        public void SpecificityIsCorrect()
        {
            var tested = new ReceiverOperatorCharacteristic(_expectedList, _resultsList);

            // Assert
            Assert.Equal(.666666666666666, tested.Specificity, 10);
        }
    }
}
