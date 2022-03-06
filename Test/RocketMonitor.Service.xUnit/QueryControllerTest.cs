using RocketMonitor.Infrastructure.Interface;
using Xunit;

namespace RocketMonitor.Service.xUnit;

public class QueryControllerTest
{
    [Fact]
    public void Should_Give_Messages_Correct_Order()
    {
        var qc = new QueryController(SetupTestEs(), new RocketConfiguration());
    }


    private static IEventStore SetupTestEs()
    {
        var es = new MemoryEventStore.MemoryEventStore();
        return es;
    }
}