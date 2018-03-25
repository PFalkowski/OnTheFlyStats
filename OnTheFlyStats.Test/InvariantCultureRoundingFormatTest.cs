using System.Text;
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

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(-1)]
        [InlineData(7)]
        [InlineData(-141234)]
        [InlineData(15)]
        public void IntsPassThroughWhenUsingAppendFormat(int input)
        {
            var tested = new InvariantCultureRoundingFormat();

            var stb = new StringBuilder();
            stb.AppendFormat(tested, "{0}", input);
            Assert.Equal(input.ToString(), stb.ToString());
        }

        [Theory]
        [InlineData("98CD27B1-011A-436D-9E3B-3AF8F21BE88F")]
        [InlineData("A9DA9D6B-26BC-4670-BE2E-8CEB75623600")]
        [InlineData("3B49D899-EF77-4772-AAB5-2196FDD94F9B")]
        [InlineData("7FA4E38F-B335-4136-BC1D-0132460F01FA")]
        public void StringsGetThroughWhenUsingAppendFormat(string input)
        {
            var tested = new InvariantCultureRoundingFormat();

            var stb = new StringBuilder();
            stb.AppendFormat(tested, "{0}", input);
            Assert.Equal(input.ToString(), stb.ToString());
        }
    }
}
