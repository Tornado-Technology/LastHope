using Content.Shared;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Client.Gameplay;

public sealed class GameplayState : GameplayStateBase
{
    [Dependency] private readonly IConfigurationManager _configuration = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    
    private FpsCounter _fpsCounter = default!;
    
    public GameplayState()
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Startup()
    {
        base.Startup();

        _fpsCounter = new FpsCounter(_gameTiming);
        _fpsCounter.Visible = _configuration.GetCVar(GameConfigVars.HudFpsCounterVisible);
        
        _userInterface.PopupRoot.AddChild(_fpsCounter);
        
        _configuration.OnValueChanged(GameConfigVars.HudFpsCounterVisible, OnHudFpsVisibleChanged);
    }

    protected override void Shutdown()
    {
        base.Shutdown();
        
        _fpsCounter.Dispose();
    }
    
    private void OnHudFpsVisibleChanged(bool value)
    {
        _fpsCounter.Visible = value;
    }
}