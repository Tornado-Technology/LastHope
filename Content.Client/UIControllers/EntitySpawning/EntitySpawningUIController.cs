using Content.Client.States.Gameplay;
using Content.Client.UIControllers.EntitySpawning.UI;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.Placement;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Input.Binding;
using Robust.Shared.Prototypes;

namespace Content.Client.UIControllers.EntitySpawning;

public sealed partial class EntitySpawningUIController : UIController, IOnStateChanged<GameplayState>
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IPlacementManager _placement = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IResourceCache _resources = default!;
    
    private EntitySpawnWindow Window
    {
        get
        {
            if (_window is not null)
                return _window;

            _window = UIManager.CreateWindow<EntitySpawnWindow>();
            _window.ReplaceButton.Pressed = _placement.Replacement;
            _window.EraseButton.Pressed = _placement.Eraser;
        
            _window.OnClose += WindowClosed;
            _window.OnResized += WindowResized;
        
            _window.ReplaceButton.OnToggled += ReplaceToggled;
            _window.EraseButton.OnToggled += EraseToggled;

            _window.ClearButton.OnPressed += ClearPressed;
            _window.SearchBar.OnTextChanged += SearchTextChanged;
        
            _window.OverrideMenu.OnItemSelected += OverrideSelected;
            _window.PrototypeScrollContainer.OnScrolled += ContainerScrolled;
            
            BuildEntityList();
            
            return _window;
        }
    }

    private readonly List<EntityPrototype> _shownEntities = new();

    private EntitySpawnWindow? _window;

    // The indices of the visible prototypes last time UpdateVisiblePrototypes was ran.
    // This is inclusive, so end is the index of the last prototype, not right after it.
    private (int start, int end) _lastEntityIndices;
    
    public override void Initialize()
    {
        base.Initialize();
        
       

    }

    public void OnStateEntered(GameplayState state)
    {
        _inputManager.SetInputCommand(ContentKeyFunctions.ToggleEntitySpawnWindow, InputCmdHandler.FromDelegate(_ =>
        {
            ToggleWindow();
        }));
    }

    public void OnStateExited(GameplayState state)
    {
        CloseWindow();
    }
    
    private void ToggleWindow()
    {
        if (Window.IsOpen)
        {
            Window.Close();
            return;
        }
        
        Window.Open();
    }

    private void CloseWindow()
    {
        Window.Close();
    }
}