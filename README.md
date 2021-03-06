# Rocket Monitor

## Get Started

The project can be run in several ways:
### Prebuilt executables

I've provided prebuilt self-contained executables for different frameworks in the github
repository **[releases](https://github.com/emilthaudal/RocketMonitor/releases)**.

The prebuild executables run on URLs: http://localhost:8088 by default. 
This can be changed by overriding the configuration using an [appsettings.json](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-6.0#override-configuration)
file or setting ```ASPNETCORE_URLS``` environment value to a different url.

**NOTE**: The API runs on http not https for now to avoid certificate setup for now. This is **NOT** production-ready
behaviour.

The executables have been tested on MacOS 12.2.1 and Windows 11.

### Docker

The RocketMonitor folder contains a Dockerfile that can be run to run the API in a Linux Docker container.

``` docker build -f RocketMonitor/Dockerfile . -t rocketmonitor ```

``` docker run -d -p 8088:5000 --name rocketm rocketmonitor ```

Populate with data with the following command:

``` ./rockets launch "http://localhost:8088/messages" --message-delay=500ms --concurrency-level=1 ```

### .NET

To build and run the project using command:

```dotnet run --project ./RocketMonitor/RocketMonitor.csproj```

Populate with data using the following command:

```./rockets launch "http://localhost:8088/messages" --message-delay=500ms --concurrency-level=1```

To run the automated test suites run following command:

```dotnet test```

The automated test suites are also run on pushes to GitHub and results can be seen here:

**[Test Results](https://github.com/emilthaudal/RocketMonitor/actions)**

## Solution architecture

Based on the assignment i wanted to make an event sourcing based solution that will allow me to store all received
messages and read them in correct order when needed to.

The solution consists of seven projects:

### RocketMonitor

This is the service that presents the API containing four endpoints:

* /messages - **POST** accepts messages from rockets
* /Dashboard/GetRockets - **GET** retrieves current state of all rockets
* /Dashboard/GetRocket - **GET** retrieves current state of a single rocket
* /Dashboard/GetRocketMessages - **GET** retrieves all messages for a single rocket

Documentation for each endpoint can be seen at /swagger when the API is running. 

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