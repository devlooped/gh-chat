using System.ComponentModel;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace Devlooped;

[Description("Chat with GitHub via the CLI using natural language")]
public class ChatCommand(IOptions<AzureOptions> options) : AsyncCommand<ChatCommand.ChatSettings>
{
    AzureOptions options = options.Value;

    public override Task<int> ExecuteAsync(CommandContext context, ChatSettings settings)
    {
        if (string.IsNullOrEmpty(options.Key) || string.IsNullOrEmpty(options.Endpoint))
        {
            AnsiConsole.MarkupLine($"[red]x[/] {ThisAssembly.Strings.MissingConfiguration}");
            return Task.FromResult(-1);
        }

        AnsiConsole.MarkupLine($"Key: {options.Key}");
        AnsiConsole.MarkupLine($"Endpoint: {options.Endpoint}");

        return Task.FromResult(0);
    }

    public class ChatSettings : CommandSettings
    {
        [Description("The question to ask GitHub")]
        [CommandArgument(0, "<question>")]
        public required string Question { get; set; }
    }
}
