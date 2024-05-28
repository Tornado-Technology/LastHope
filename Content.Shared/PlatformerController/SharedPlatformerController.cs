using System.Linq;
using System.Numerics;
using Content.Shared.Physics;
using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Flags;
using Content.Shared.Tag;
using Content.Shared.Utilities;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared.PlatformerController;

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
        var controller = entity.Comp1;
        var transformComponent = entity.Comp3;

        var worldPosition = _transform.GetWorldPosition(transformComponent) - new Vector2(0, 0.51f);
        var direction = new Vector2(0, -1);
        var ray = new CollisionRay(worldPosition, direction, (int)CollisionGroup.BlockMask);

        var results = _physics.IntersectRayWithPredicate(transformComponent.MapID, ray, maxLength: 0.05f, predicate: IsGround);
        controller.Grounded = results.Any();
    }

    private void UpdateJumping(Entity<PlatformerControllerComponent, PhysicsComponent, TransformComponent> entity,
        ref Vector2 velocity, float frameTime)
    {
        var controller = entity.Comp1;
        var jumpHeld = controller.HeldButtons.HasFlag(PlatformerControllerButtons.Jump);

        if (jumpHeld && controller.Grounded)
        {
            velocity.Y = controller.JumpForce;
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

        velocity.Y = Math.Max(velocity.Y, -controller.MaxFallSpeed);
    }

    private bool IsGround(EntityUid entityUid) => _tag.HasTag(entityUid, GroundTag);
}