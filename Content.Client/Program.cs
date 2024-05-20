using Robust.Client;
using Robust.Shared.Utility;

namespace Content.Client;

internal static class Program
{
    public static void Main(string[] args)
    {
        ContentStart.StartLibrary(args, new GameControllerOptions
        {
            // DEVNOTE: Your options here.
            Sandboxing = false,
            // SplashLogo = new ResourcePath("/path/to/splash/logo.png"),
            
            // Check "RobustToolbox/Resources/Textures/Logo/icon" for an example window icon set.
            // WindowIconSet = new ResourcePath("/path/to/folder/with/window/icon/set"),
            DefaultWindowTitle = "LastHope"
        });
    }
}