# SampleDotnetMcpServer

This project is a sample implementation of a Model Context Protocol (MCP) server in .NET, designed to demonstrate how to build, run, and publish an MCP server using C# and the [ModelContextProtocol SDK](https://www.nuget.org/packages/ModelContextProtocol).

mcp-name: io.github.timheuer/sampledotnetmcpserver

## What is this?


**SampleDotnetMcpServer** is a reference MCP server for .NET, showing how to:

- Implement MCP tools in C# (see `Tools/RandomNumberTools.cs`, `Tools/SayHelloName.cs`, `Tools/WeatherTools.cs`)
- Register tools and configure the MCP server transport (see `Program.cs`)
- Package and publish the server as a NuGet package for use in IDEs like VS Code and Visual Studio

The included tools demonstrate various MCP capabilities:
- `get_random_number`: Generates random numbers within a specified range
- `say_hello_name` and `say_goodbye_name`: Simple greeting tools
- `get_weather_by_zip_code`: Gets current weather information for any US zip code
- `get_weather_forecast`: Gets a multi-day weather forecast for any US zip code

## Features

- Implements the MCP protocol using stdio transport
- Provides multiple sample tools:
  - `get_random_number`: Returns a random integer in a specified range
  - `say_hello_name` and `say_goodbye_name`: Simple greeting tools
  - `get_weather_by_zip_code`: Gets current weather for any US zip code using wttr.in API
  - `get_weather_forecast`: Gets multi-day weather forecast for any US zip code
- Ready to be packed and published as a NuGet MCP server
- Example configuration files and metadata included

## How to use

### Run locally

You can run the MCP server directly from source for development and testing:

```shell
dotnet run --project .
```

Or configure your IDE to use the following MCP server definition:

```json
{
  "servers": {
    "SampleDotnetMcpServer": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "<PATH TO PROJECT DIRECTORY>"
      ]
    }
  }
}
```

### Test the MCP server

Once running, you can use Copilot Chat or any MCP client to invoke the available tools:

**Weather Tools:**
- Ask: "What's the weather like in 90210?" to get current weather
- Ask: "Give me a 3-day forecast for 10001" to get a weather forecast
- Ask: "Check the weather in 60601" to test with Chicago's zip code

**Other Tools:**
- Ask: "Give me 3 random numbers" to use the random number generator
- Ask: "Say hello to John" to use the greeting tools


### Publish to NuGet.org

CI handles publishing to NuGet, using NuGet Trusted Publishing for the key and automatic versioning using Nerdbank.GitVersioning.

### Use from NuGet.org

After publishing, configure your IDE to use the MCP server from NuGet.org. Example configuration (replace package id and version):

```json
{
  "servers": {
    "SampleDotnetMcpServer": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "TimHeuer.SampleMcpServer",
        "--version",
        "0.1.0-beta",
        "--yes"
      ]
    }
  }
}
```

## Project structure

- `Program.cs`: MCP server entry point and configuration
- `Tools/RandomNumberTools.cs`: Example MCP tool for generating random numbers
- `Tools/SayHelloName.cs`: Example MCP tools for greetings
- `Tools/WeatherTools.cs`: Weather-related MCP tools for checking weather by zip code
- `.mcp/server.json`: MCP server metadata and NuGet package info
- `SampleDotnetMcpServer.csproj`: Project and NuGet packaging configuration

## Learn more

- [ModelContextProtocol SDK](https://www.nuget.org/packages/ModelContextProtocol)
- [Model Context Protocol Documentation](https://modelcontextprotocol.io/)
- [Protocol Specification](https://spec.modelcontextprotocol.io/)
- [GitHub Organization](https://github.com/modelcontextprotocol)
- [Use MCP servers in VS Code (Preview)](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Use MCP servers in Visual Studio (Preview)](https://learn.microsoft.com/visualstudio/ide/mcp-servers)

---

This project is for demonstration and reference purposes only. For feedback or contributions, see the [GitHub repository](https://github.com/timheuer/sampledotnetmcpserver).
