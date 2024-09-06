using System.ComponentModel;
using DotNetConfig;
using Spectre.Console;

namespace Devlooped;

[Description("Manages configuration")]
public class ConfigCommand(Config config) : Command<ConfigCommand.ConfigSettings>
{
    public override int Execute(CommandContext context, ConfigSettings settings)
    {
        config.SetString("gh-chat", "key", settings.Key);
        config.SetString("gh-chat", "endpoint", settings.Endpoint);

        AnsiConsole.MarkupLine($"[green]✓[/] {ThisAssembly.Strings.ConfigurationSaved}");
        return 0;
    }

    public class ConfigSettings : CommandSettings
    {
        [Description("Azure OpenAI key to use")]
        [CommandOption("-k|--key")]
        public required string Key { get; set; }

        [Description("Azure OpenAI endpoint to use")]
        [CommandOption("-e|--endpoint|")]
        public required string Endpoint { get; set; }

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Key))
                return ValidationResult.Error("Key is required.");

            if (string.IsNullOrWhiteSpace(Endpoint))
                return ValidationResult.Error("Endpoint is required.");

            return base.Validate();
        }
    }
}
