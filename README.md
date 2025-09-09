
# SampleDotnetMcpServer

This project is a sample implementation of a Model Context Protocol (MCP) server in .NET, designed to demonstrate how to build, run, and publish an MCP server using C# and the [ModelContextProtocol SDK](https://www.nuget.org/packages/ModelContextProtocol).

mcp-name: io.github.timheuer/sampledotnetmcpserver

## What is this?


**SampleDotnetMcpServer** is a reference MCP server for .NET, showing how to:

- Implement MCP tools in C# (see `Tools/RandomNumberTools.cs`)
- Register tools and configure the MCP server transport (see `Program.cs`)
- Package and publish the server as a NuGet package for use in IDEs like VS Code and Visual Studio

The included tool, `get_random_number`, generates random numbers and can be invoked by MCP clients (such as Copilot Chat) for demonstration purposes.

## Features

- Implements the MCP protocol using stdio transport
- Provides a sample tool: `get_random_number` (returns a random integer in a range)
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

Once running, you can use Copilot Chat or any MCP client to invoke the `get_random_number` tool. For example, ask: "Give me 3 random numbers" and select the tool from the SampleDotnetMcpServer MCP server.

### Publish to NuGet.org



1. Run:

  ```shell
  dotnet pack -c Release
  ```

1. Publish:

  ```shell
  dotnet nuget push bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json
  ```

### Use from NuGet.org

After publishing, configure your IDE to use the MCP server from NuGet.org. Example configuration:

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
- `Tools/RandomNumberTools.cs`: Example MCP tool implementation
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
