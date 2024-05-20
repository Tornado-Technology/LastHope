using Robust.Server;

namespace Content.Server;

internal static class Program
{
    public static void Main(string[] args)
    {
        ContentStart.StartLibrary(args, new ServerOptions
        {
            // DEVNOTE: Your options here.
            Sandboxing = false,
        });
    }
}