using Xunit;

namespace OnTheFlyStats.Test
{
    public class InvariantCultureRoundingFormatTest
    {
        [Theory]
        [InlineData(1.0, "1")]
        [InlineData(3.14, "3.14")]
        [InlineData(-1, "-1")]
        [InlineData(7.0001, "7")]
        [InlineData(-141234.78913, "-141234.79")]
        [InlineData(15.0, "15")]
        public void DoublesGetRounded(double input, string expected)
        {
            var tested = new InvariantCultureRoundingFormat();
            var result = tested.Format("", input, null);

            Assert.Equal(expected, result);
        }
    }
}
