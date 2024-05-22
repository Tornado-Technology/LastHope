using System.Numerics;
using Content.Shared.Movement.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Timing;

namespace Content.Shared.Movement;

public abstract partial class SharedMovementController : VirtualController
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    
    private EntityQuery<MovementComponent> _movementQuery;
    private EntityQuery<MovementSpeedComponent> _movementSpeedQuery;
    private EntityQuery<TransformComponent> _transformQuery;
    
    public override void Initialize()
    {
        base.Initialize();

        _movementQuery = GetEntityQuery<MovementComponent>();
        _movementSpeedQuery = GetEntityQuery<MovementSpeedComponent>();
        _transformQuery = GetEntityQuery<TransformComponent>();
        
        InitializeInput();
    }

    protected void HandleMovement(Entity<MovementComponent, PhysicsComponent, TransformComponent> movement, float frameTime)
    {
        // TODO: Can move logic here, for some cool effects
        var physicsComponent = movement.Comp2;
        
        // Get movement speed
        var moveSpeed = _movementSpeedQuery.CompOrNull(movement);
        var walkSpeed = moveSpeed?.WalkSpeed ?? MovementSpeedComponent.DefaultBaseWalkSpeed;
        var sprintSpeed = moveSpeed?.SprintSpeed ?? MovementSpeedComponent.DefaultBaseSprintSpeed;
        
        var total = GetVelocityInput(movement, walkSpeed, sprintSpeed);
        var hasInput = total == Vector2.Zero;
        
        // Get velocity changers
        var acceleration = moveSpeed?.Acceleration ?? MovementSpeedComponent.DefaultAcceleration;
        var friction = hasInput
            ? moveSpeed?.Friction ?? MovementSpeedComponent.DefaultFriction
            : moveSpeed?.FrictionNoInput ?? MovementSpeedComponent.DefaultFrictionNoInput;

        var velocity = physicsComponent.LinearVelocity;
        
        Friction(ref velocity, friction, frameTime);
        Accelerate(ref velocity, ref total, acceleration, frameTime);
        
        PhysicsSystem.SetLinearVelocity(movement, velocity, body: movement);
        PhysicsSystem.SetAngularVelocity(movement, 0, body: movement);
    }

    private void Friction(ref Vector2 velocity, float friction, float frameTime)
    {
        var speed = velocity.Length();
        var drop = 0f;
        
        var control = MathF.Max(0f, speed);
        
        drop += control * friction * frameTime;

        var newSpeed = MathF.Max(0f, speed - drop);
        if (newSpeed.Equals(speed))
            return;

        newSpeed /= speed;
        velocity *= newSpeed;
    }
    
    private void Accelerate(ref Vector2 velocity, ref Vector2 total, float acceleration, float frameTime)
    {
        var wishDir = total != Vector2.Zero ? total.Normalized() : Vector2.Zero;
        var wishSpeed = total.Length();

        var currentSpeed = Vector2.Dot(velocity, wishDir);
        var addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0f)
            return;

        var accelerationSpeed = acceleration * frameTime * wishSpeed;
        accelerationSpeed = MathF.Min(accelerationSpeed, addSpeed);

        velocity += wishDir * accelerationSpeed;
    }
}