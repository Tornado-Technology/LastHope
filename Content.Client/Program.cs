using Robust.Client;

namespace Content.Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        ContentStart.StartLibrary(args, new GameControllerOptions
        {
            Sandboxing = false,
            UserDataDirectoryName = "LastHope",
            DefaultWindowTitle = "LastHope"
        });
    }
}