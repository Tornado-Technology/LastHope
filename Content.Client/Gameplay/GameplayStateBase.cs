using System.Linq;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Client.Gameplay;

public abstract partial class GameplayStateBase : Robust.Client.State.State, IEntityEventSubscriber
{
    private const string VvmDomain = "enthover";
    
    [Dependency] private readonly IEyeManager _eyeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IViewVariablesManager _vvm = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    
    protected override void Startup()
    {
        _vvm.RegisterDomain(VvmDomain, ResolveVvHoverObject, ListVvHoverPaths);
        _inputManager.KeyBindStateChanged += OnKeyBindStateChanged;
    }

    protected override void Shutdown()
    {
        _vvm.UnregisterDomain(VvmDomain);
        _inputManager.KeyBindStateChanged -= OnKeyBindStateChanged;
    }
    
    public EntityUid? GetClickedEntity(MapCoordinates coordinates)
    {
        var first = GetClickableEntities(coordinates).FirstOrDefault();
        return first.IsValid() ? first : null;
    }
}