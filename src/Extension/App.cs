using DotNetConfig;
using Microsoft.Extensions.DependencyInjection;

namespace Devlooped;

public static class App
{
    public static CommandApp Create()
    {
        var builder = new ServiceCollection();
        var cfgdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "gh-chat");
        Directory.CreateDirectory(cfgdir);

        // Made transient so each command gets a new copy with potentially updated values.
        builder.AddTransient(sp => Config.Build(cfgdir));

        var registrar = new TypeRegistrar(builder);
        var app = new CommandApp(registrar);

        builder.AddSingleton<ICommandApp>(app);

        app.Configure(config => config.SetApplicationName("gh chat"));

        // TODO: add commands, etc.

        return app;
    }
}
