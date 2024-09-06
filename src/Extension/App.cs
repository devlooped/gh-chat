using DotNetConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devlooped;

public static class App
{
    public static ICommandApp Create()
    {
        var builder = new ServiceCollection();
        var cfgdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "gh-chat");
        Directory.CreateDirectory(cfgdir);

        builder
            .AddOptions<AzureOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.GetSection("gh-chat").Bind(options));

        // Made transient so each command gets a new copy with potentially updated values.
        builder.AddTransient(sp => Config.Build(cfgdir));
        builder.AddTransient<IConfiguration>(sp => new ConfigurationBuilder()
            .AddDotNetConfig(cfgdir)
            .Build());

        var registrar = new TypeRegistrar(builder);
        var app = new CommandApp<ChatCommand>(registrar);

        builder.AddSingleton<ICommandApp>(app);

        app.Configure(config =>
        {
            config.SetApplicationName("gh chat");
            config.AddCommand<ConfigCommand>("config");
        });

        // TODO: add commands, etc.

        return app;
    }
}
