using System.Linq;
using System.Numerics;
using Robust.Client.ComponentTrees;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Shared.Map;

namespace Content.Client.Gameplay;

public abstract partial class GameplayStateBase : Robust.Client.State.State, IEntityEventSubscriber
{
    private const string VvmDomain = "enthover";
    
    [Dependency] private readonly IEyeManager _eyeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IViewVariablesManager _vvm = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    
    protected override void Startup()
    {
        _vvm.RegisterDomain(VvmDomain, ResolveVvHoverObject, ListVvHoverPaths);
    }

    protected override void Shutdown()
    {
        _vvm.UnregisterDomain(VvmDomain);
    }
    
    public EntityUid? GetClickedEntity(MapCoordinates coordinates)
    {
        var first = GetClickableEntities(coordinates).FirstOrDefault();
        return first.IsValid() ? first : null;
    }
    

}