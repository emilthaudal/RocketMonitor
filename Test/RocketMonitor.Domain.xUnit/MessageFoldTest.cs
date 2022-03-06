using System;
using RocketMonitor.Domain.Json;
using RocketMonitor.Domain.Message;
using RocketMonitor.Domain.Model;
using Xunit;

namespace RocketMonitor.Domain.xUnit;

/// <summary>
///     Test suite to test .Fold methods on each message type. Verify that message content is applied to state correctly
///     and that messages only fold in legal conditions.
/// </summary>
public class MessageFoldTest
{
    [Fact]
    public void Should_Not_Fold_RocketExploded_Without_Rocket_Launched_Message()
    {
        var channel = Guid.NewGuid();
        var rocket = new Rocket();
        var explosionTime = new DateTime(2022, 1, 1);
        var explodedMessage = new RocketExploded
        {
            Metadata = new Metadata
            {
                Channel = channel,
                MessageNumber = 3,
                MessageTime = explosionTime
            },
            Reason = "Malfunction"
        };
        Assert.Throws<InvalidOperationException>(() => rocket = explodedMessage.Fold(rocket));
    }

    [Fact]
    public void Should_Fold_RockedExploded_When_Rocket_Launched_Exist()
    {
        var rocketLaunchedMessage = CreateRockedLaunchedMessage();
        var rocket = rocketLaunchedMessage.Fold(new Rocket());
        var explosionTime = new DateTime(2022, 1, 1);
        const string reason = "Malfunction";
        var explodedMessage = new RocketExploded
        {
            Metadata = new Metadata
            {
                Channel = rocket.Channel,
                MessageNumber = 3,
                MessageTime = explosionTime
            },
            Reason = reason
        };
        rocket = explodedMessage.Fold(rocket);
        Assert.NotNull(rocket);
        Assert.Equal(explosionTime, rocket.ExplodedTime);
        Assert.Equal(reason, rocket.ExplodedReason);
    }

    [Fact]
    public void Should_Not_Fold_RockedMissionChanged_Without_Rocket_Launched_Message()
    {
        var channel = Guid.NewGuid();
        var rocket = new Rocket();
        var missionChanged = new RocketMissionChanged
        {
            Metadata = new Metadata
            {
                Channel = channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            NewMission = "Mission"
        };
        Assert.Throws<InvalidOperationException>(() => rocket = missionChanged.Fold(rocket));
    }

    [Fact]
    public void Should_Fold_RockedMissionChanged_When_Rocket_Launched_Exist()
    {
        var rocketLaunchedMessage = CreateRockedLaunchedMessage();
        var rocket = rocketLaunchedMessage.Fold(new Rocket());
        const string mission = "newMission";
        var missionChanged = new RocketMissionChanged
        {
            Metadata = new Metadata
            {
                Channel = rocket.Channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            NewMission = mission
        };
        rocket = missionChanged.Fold(rocket);
        Assert.NotNull(rocket);
        Assert.Equal(mission, rocket.Mission);
    }

    [Fact]
    public void Should_Not_Fold_RocketSpeedDecreased_Without_Rocket_Launched_Message()
    {
        var channel = Guid.NewGuid();
        var rocket = new Rocket();
        var speedDecreased = new RocketSpeedDecreased
        {
            Metadata = new Metadata
            {
                Channel = channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            By = 500
        };
        Assert.Throws<InvalidOperationException>(() => rocket = speedDecreased.Fold(rocket));
    }

    [Fact]
    public void Should_Fold_RocketSpeedDecreased_When_Rocket_Launched_Exist()
    {
        var rocketLaunchedMessage = CreateRockedLaunchedMessage();
        var rocket = rocketLaunchedMessage.Fold(new Rocket());
        var initialSpeed = rocket.LaunchSpeed;
        const double speedDecreased = 100;
        var rocketSpeedDecreased = new RocketSpeedDecreased
        {
            Metadata = new Metadata
            {
                Channel = rocket.Channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            By = speedDecreased
        };
        rocket = rocketSpeedDecreased.Fold(rocket);
        Assert.NotNull(rocket);
        Assert.Equal(initialSpeed - speedDecreased, rocket.LaunchSpeed);
    }

    [Fact]
    public void Should_Not_Fold_RocketSpeedIncreased_Without_Rocket_Launched_Message()
    {
        var channel = Guid.NewGuid();
        var rocket = new Rocket();
        var speedDecreased = new RocketSpeedIncreased
        {
            Metadata = new Metadata
            {
                Channel = channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            By = 500
        };
        Assert.Throws<InvalidOperationException>(() => rocket = speedDecreased.Fold(rocket));
    }

    [Fact]
    public void Should_Fold_RocketSpeedIncreased_When_Rocket_Launched_Exist()
    {
        var rocketLaunchedMessage = CreateRockedLaunchedMessage();
        var rocket = rocketLaunchedMessage.Fold(new Rocket());
        var initialSpeed = rocket.LaunchSpeed;
        const double increase = 100;
        var rocketSpeedIncreased = new RocketSpeedIncreased
        {
            Metadata = new Metadata
            {
                Channel = rocket.Channel,
                MessageNumber = 3,
                MessageTime = DateTime.Now
            },
            By = increase
        };
        rocket = rocketSpeedIncreased.Fold(rocket);
        Assert.NotNull(rocket);
        Assert.Equal(initialSpeed + increase, rocket.LaunchSpeed);
    }


    private static RocketLaunched CreateRockedLaunchedMessage()
    {
        return new RocketLaunched
        {
            LaunchSpeed = 500,
            Mission = "New Mission",
            Type = "RocketType",
            Metadata = new Metadata
            {
                Channel = Guid.NewGuid(),
                MessageNumber = 1,
                MessageTime = DateTime.Now
            }
        };
    }
}