using Content.Shared.Movement.Handlers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Physics.Controllers;

namespace Content.Shared.Movement;

public abstract class SharedMovementController : VirtualController
{
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;
    
    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _log.GetSawmill(SawmillName);
        
        CommandBinds.Builder
            .Bind(EngineKeyFunctions.MoveLeft, new MovementHorizontalCmdHandler(this, Direction.West))
            .Bind(EngineKeyFunctions.MoveRight, new MovementHorizontalCmdHandler(this, Direction.East))
            .Register<SharedMovementController>();
    }

    public void HandleHorizontal(EntityUid entityUid, Direction direction, bool state)
    {
        _sawmill.Info($"{state}");
    }
}