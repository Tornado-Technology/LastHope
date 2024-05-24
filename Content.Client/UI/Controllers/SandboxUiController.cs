using Content.Client.States.Gameplay;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Shared.Input.Binding;

namespace Content.Client.UI.Controllers;

public sealed class SandboxUiController : UIController, IOnStateChanged<GameplayState>
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    
    private EntitySpawningUIController _entitySpawningController = default!;
    private TileSpawningUIController _tileSpawningController = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        _entitySpawningController = UIManager.GetUIController<EntitySpawningUIController>();
        _tileSpawningController = UIManager.GetUIController<TileSpawningUIController>();
    }

    public void OnStateEntered(GameplayState state)
    {
        _inputManager.SetInputCommand(ContentKeyFunctions.ToggleEntitySpawnWindow, InputCmdHandler.FromDelegate(_ =>
        {
            _entitySpawningController.ToggleWindow();
        }));
        
        _inputManager.SetInputCommand(ContentKeyFunctions.ToggleTileSpawnWindow, InputCmdHandler.FromDelegate(_ =>
        {
            _tileSpawningController.ToggleWindow();
        }));
    }

    public void OnStateExited(GameplayState state)
    {
        _entitySpawningController.CloseWindow();
        _tileSpawningController.CloseWindow();
    }
}