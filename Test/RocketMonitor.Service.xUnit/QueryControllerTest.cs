using System;
using RocketMonitor.Infrastructure.Interface;
using RocketMonitor.MemoryEventStore.Model;
using Xunit;

namespace RocketMonitor.Service.xUnit;

/// <summary>
///     Test suite with tests verifying functionality of QueryController
///     Main functionality to test is correct handling of messages received in wrong order
/// </summary>
public class QueryControllerTest
{
    [Fact]
    public void Should_Give_Messages_Correct_Order()
    {
        var channel = Guid.NewGuid();
        var stream = $"stream-{channel:N}";
        var qc = new QueryController(SetupTestEs(stream), new RocketConfiguration());
    }


    private static IEventStore SetupTestEs(string stream)
    {
        var es = new MemoryEventStore.MemoryEventStore();
        // TODO Create test data to verify QueryController
        var e1 = new Event();
        var e2 = new Event();
        var e3 = new Event();
        es.AppendStream(stream, e1);
        es.AppendStream(stream, e2);
        es.AppendStream(stream, e3);

        return es;
    }
}