using Content.Shared.Input;
using Robust.Shared.Input;

namespace Content.Client.Input;

public static class ContentContexts
{
    private const string DefaultContextName = "common";
    private const string PlayerContextName = "player";
    
    public static void SetupContexts(IInputContextContainer contexts)
    {
        var common = contexts.GetContext(DefaultContextName);

        common.AddFunction(EngineKeyFunctions.MoveLeft);
        common.AddFunction(EngineKeyFunctions.MoveRight);
        common.AddFunction(ContentKeyFunctions.Jump);

        var player = contexts.New(PlayerContextName, DefaultContextName);
    }
}