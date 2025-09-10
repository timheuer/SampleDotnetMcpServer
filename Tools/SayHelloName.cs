using System.ComponentModel;
using ModelContextProtocol.Server;

/// <summary>
/// Sample MCP tools for demonstration purposes.
/// These tools can be invoked by MCP clients to perform various operations.
/// </summary>
internal class HelloTools
{
    [McpServerTool]
    [Description("Returns a string with the name requested when someone asks to say hello world with their name.")]
    public string SayHelloName(
        [Description("The name to say hello to")] string name)
    {
        return $"Hello World, {name}";
    }
}
