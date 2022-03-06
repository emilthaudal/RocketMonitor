# Solution to Lunar Backend Challenge

## Get Started
The project can be run in several ways:
### Prebuilt executables
I've provided prebuilt executables for different frameworks in the corresponding folders.

### Docker
The RocketMonitor.API folder contains a Dockerfile that can be run to run
the API in a Linux Docker container.

### .NET
To build and run the project using command:

```dotnet run --project ./RocketMonitor.API/RocketMonitor.API.csproj```


## Solution architecture
Based on the assignment i wanted to make an event sourcing based solution 
that will allow me to store all received messages and read them in correct order when needed to.

The solution consists of five projects:

### RocketMonitor.API
This is the service that presents the API containing three endpoints:
* /messages
* /api/v1/GetRockets
* /api/v1/GetRocket

Documentation for each endpoint can be seen at /swagger when the API is running.
The default URL is https://localhost:8088

### RocketMonitor.Domain
Library project containing models and messages.

### RocketMonitor.Infrastructure
Library project with interfaces for required infrastructure such as 
IEventStore describing the needed Event sourcing datastore.

### RocketMonitor.MemoryEventStore
A simple in-memory Event Store i've implemented for the solution. 
Stores events in  a ConcurrentDictionary grouped by stream-ids (Rocket channels).

### RocketMonitor.Service
Service library containing Query and CommandController to read and write
messages to the Event Store.

### RocketMonitor.Service.xUnit
Unit-test project containing tests that verify functionality of the Query and CommandController.
