# Solution to Lunar Backend Challenge

## Get Started

The project can be run in several ways:

### Prebuilt executables

I've provided prebuilt self-contained executables for different frameworks in the github
repository **[releases](https://github.com/emilthaudal/RocketMonitor/releases)**.

### Docker

The RocketMonitor folder contains a Dockerfile that can be run to run the API in a Linux Docker container.

``` docker build -f RocketMonitor/Dockerfile . -t rocketmonitor```

```docker run -d -p 8088:5000 --name rocketm rocketmonitor```

**NOTE**: The API runs on http not https in Docker to avoid certificate setup for now. This is **not** production-ready
behaviour.

### .NET

To build and run the project using command:

```dotnet run --project ./RocketMonitor/RocketMonitor.csproj```

To run the test suite run following command:

```dotnet test```

## Solution architecture

Based on the assignment i wanted to make an event sourcing based solution that will allow me to store all received
messages and read them in correct order when needed to.

The solution consists of seven projects:

### RocketMonitor

This is the service that presents the API containing three endpoints:

* /messages - **POST** accepts messages from rockets
* /Dashboard/GetRockets - **GET** retrieves current state of all rockets
* /Dashboard/GetRocket - **GET** retrieves current state of a single rocket
* /Dashboard/GetRocketMessages - **GET** retrieves all messages for a single rocket

Documentation for each endpoint can be seen at /swagger when the API is running. The default URLs
are https://localhost:8088 and http://localhost:8089

### RocketMonitor.Domain

Library project containing models and messages.

### RocketMonitor.Infrastructure

Library project with interfaces for required infrastructure such as IEventStore describing the needed Event sourcing
datastore.

### RocketMonitor.MemoryEventStore

A simple in-memory Event Store i've implemented for the solution. Stores events in a ConcurrentDictionary grouped by
stream-ids (Rocket channels).

### RocketMonitor.Service

Service library containing Query and CommandController to read and write messages to the Event Store.

## Test

### RocketMonitor.Service.xUnit

Unit-test project containing tests that verify functionality of the Query and CommandController.

### RocketMonitor.Domain.xUnit

Unit-test project testing various validations on the models and messages.