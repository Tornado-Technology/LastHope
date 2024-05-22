using Content.Shared.Movement.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Timing;

namespace Content.Shared.Movement;

public abstract partial class SharedMovementController : VirtualController
{
    [Dependency] private readonly IGameTiming _timing = default!;
    
    private EntityQuery<MovementComponent> _movementQuery;
    private EntityQuery<MovementSpeedComponent> _movementSpeedQuery;
    
    public override void Initialize()
    {
        base.Initialize();

        _movementQuery = GetEntityQuery<MovementComponent>();
        _movementSpeedQuery = GetEntityQuery<MovementSpeedComponent>();
        
        InitializeInput();
    }
}