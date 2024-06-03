using System.Linq;
using System.Numerics;
using Content.Shared.Physics;
using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Events;
using Content.Shared.PlatformerController.Flags;
using Content.Shared.Tag;
using Content.Shared.Utilities;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared.PlatformerController;

// TODO: It's still a big piece of crap, need to rewrite this in the future
public abstract partial class SharedPlatformerController : VirtualController
{
    private static readonly ProtoId<TagPrototype> GroundTag = "Ground";

    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    
    private EntityQuery<PlatformerControllerComponent> _platformerControllerQuery;
    
    public override void Initialize()
    {
        base.Initialize();
        
        _platformerControllerQuery = GetEntityQuery<PlatformerControllerComponent>();

        InitializeInput();
        InitializeJumping();
        InitializeMovement();
    }

    protected void UpdateEntity(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        float frameTime)
    {
        var physicsComponent = entity.Comp2;
        var velocity = physicsComponent.LinearVelocity;

        UpdateCollisions(entity, frameTime);
        
        UpdateJumping(entity, ref velocity, frameTime);
        UpdateMovement(entity, ref velocity, frameTime);
        UpdateGravity(entity, ref velocity, frameTime);
        
        _physics.SetLinearVelocity(entity, velocity);
    }

    private void UpdateCollisions(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity, float frameTime)
    {
        var transformComponent = entity.Comp3;

        var worldPosition = _transform.GetWorldPosition(transformComponent) - new Vector2(0, 0.51f);
        var direction = new Vector2(0, -1);
        var ray = new CollisionRay(worldPosition, direction, (int)CollisionGroup.BlockMask);

        var results = _physics.IntersectRayWithPredicate(transformComponent.MapID, ray, maxLength: 0.05f, predicate: IsGround);
        SetGrounded(entity, results.Any());
    }

    private void UpdateJumping(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        ref Vector2 velocity, float frameTime)
    {
        var controllerComponent = entity.Comp1;
        
        var jumpHeld = controllerComponent.HeldButtons.HasFlag(PlatformerControllerButtons.Jump);

        if (controllerComponent is { EndedJumpEarly: false, Grounded: false } && !jumpHeld && velocity.Y > 0)
            controllerComponent.EndedJumpEarly = true;
        
        if (jumpHeld && controllerComponent.Grounded)
        {
            Jump(entity, ref velocity, frameTime);
        }
    }
    
    private void UpdateMovement(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        ref Vector2 velocity, float frameTime)
    {
        var controller = entity.Comp1;

        if (controller.Movement.X == 0)
        {
            var friction = controller.Grounded ? controller.Friction : controller.AirFriction;
            velocity.X = Mathf.MoveTowards(velocity.X, 0, friction * frameTime);
            return;
        }

        velocity.X = Mathf.MoveTowards(velocity.X, controller.Movement.X * controller.MaxSpeed,
            controller.Acceleration * frameTime);
    }

    private void UpdateGravity(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        ref Vector2 velocity, float frameTime)
    {
        var controller = entity.Comp1;

        // We're flying up here, most likely from the jump.
        if (velocity.Y > 0)
        {
            // Allows us to better control the jump
            if (controller.EndedJumpEarly)
                velocity.Y -= velocity.Y * controller.JumpEndEarlyGravityModifier;
        }
        
        // We are falling here
        if (velocity.Y < 0)
        {
            // Limit the maximum rate at which an entity can fall
            velocity.Y = Math.Max(velocity.Y, -controller.MaxFallSpeed);
        }
    }

    private void Jump(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        ref Vector2 velocity, float frameTime)
    {
        var controllerComponent = entity.Comp1;

        controllerComponent.EndedJumpEarly = false;
        
        velocity.Y = controllerComponent.JumpForce;
    }

    private bool IsGround(EntityUid entityUid) => _tag.HasTag(entityUid, GroundTag);
    
    private void SetGrounded(Entity<PlatformerControllerComponent> entity, bool grounded)
    {
        if (entity.Comp.Grounded == grounded)
            return;

        entity.Comp.Grounded = grounded;
        
        if (!grounded)
            return;
        
        var ev = new GroundedPlatformerControllerEvent();
        RaiseLocalEvent(entity, ref ev);
    }
}