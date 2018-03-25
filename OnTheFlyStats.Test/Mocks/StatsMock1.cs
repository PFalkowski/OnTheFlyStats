using Xunit;

namespace OnTheFlyStats.Test.Mocks
{
    public class StatsMock1 : TheoryData<Stats>
    {
        public StatsMock1()
        {
            var result = new Stats();
            result.Update(1.0);
            result.Update(2);
            result.Update(3);
            result.Update(3.14);
            result.Update(4);
            result.Update(1);
            result.Update(-1);
            result.Update(7);
            result.Update(-141234);
            result.Update(15);
            Add(result);
        }
    }
}
