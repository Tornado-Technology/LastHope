using Content.Client.Input;
using Content.Client.States;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.State;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client;

[UsedImplicitly]
public sealed class EntryPoint : GameClient
{
    public override void Init()
    {
        var factory = IoCManager.Resolve<IComponentFactory>();
        var prototypes = IoCManager.Resolve<IPrototypeManager>();

        factory.DoAutoRegistrations();

        foreach (var ignoreName in IgnoredComponents.List)
        {
            factory.RegisterIgnore(ignoreName);
        }

        foreach (var ignoreName in IgnoredPrototypes.List)
        {
            prototypes.RegisterIgnore(ignoreName);
        }

        ClientContentIoC.Register();

        IoCManager.BuildGraph();

        factory.GenerateNetIds();
    }

    public override void PostInit()
    {
        base.PostInit();

        var inputManager = IoCManager.Resolve<IInputManager>(); 
        var stateManager = IoCManager.Resolve<IStateManager>();
        
        // Setup input context
        ContentContexts.SetupContexts(inputManager.Contexts);
        
        IoCManager.Resolve<ILightManager>().Enabled = false;

    
        
        stateManager.RequestStateChange<MainScreenState>();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);
    }
}