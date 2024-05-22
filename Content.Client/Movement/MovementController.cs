using Content.Shared.Movement;
using Content.Shared.Movement.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;

namespace Content.Client.Movement;

public sealed class MovementController : SharedMovementController
{
    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<MovementComponent, PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<MovementComponent, PlayerDetachedEvent>(OnPlayerDetached);
    }

    private void OnPlayerAttached(Entity<MovementComponent> movement, ref PlayerAttachedEvent args)
    {
        SetMoveInput(movement, MoveButtons.None);
    }

    private void OnPlayerDetached(Entity<MovementComponent> movement, ref PlayerDetachedEvent args)
    {
        SetMoveInput(movement, MoveButtons.None);
    }
    
    public override void UpdateBeforeSolve(bool prediction, float frameTime)
    {
        base.UpdateBeforeSolve(prediction, frameTime);

        var query = EntityQueryEnumerator<MovementComponent, PhysicsComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var movementComponent, out var physicsComponent, out var transformComponent))
        {
            HandleMovement((uid, movementComponent, physicsComponent, transformComponent), frameTime);
        }
    }
}