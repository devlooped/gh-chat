using System.Diagnostics;
using Devlooped;

#if DEBUG
if (args.Contains("--debug"))
{
    Debugger.Launch();
    args = args.Where(x => x != "--debug").ToArray();
}
#endif

if (args.Contains("-?"))
    args = args.Select(x => x == "-?" ? "-h" : x).ToArray();

var app = App.Create();

return app.Run(args);