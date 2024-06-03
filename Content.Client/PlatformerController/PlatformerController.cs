using System.Numerics;
using Content.Shared.PlatformerController;
using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Events;
using Content.Shared.PlatformerController.Flags;
using Robust.Client.GameObjects;
using Robust.Shared.Physics.Components;

namespace Content.Client.PlatformerController;

// TODO: A lot of hardcoded crap, required to rewrite this later on
public sealed class PlatformerController : SharedPlatformerController
{
    private EntityQuery<SpriteComponent> _spriteQuery;
    private EntityQuery<AnimationPlatformerControllerComponent> _animationQuery;
    
    public override void Initialize()
    {
        base.Initialize();

        _spriteQuery = GetEntityQuery<SpriteComponent>();
        _animationQuery = GetEntityQuery<AnimationPlatformerControllerComponent>();
        
        SubscribeLocalEvent<AnimationPlatformerControllerComponent, MoveInputEvent>(OnMoveInput);
        SubscribeLocalEvent<AnimationPlatformerControllerComponent, GroundedPlatformerControllerEvent>(OnGrounded);
    }
    
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<PlatformerControllerComponent, PhysicsComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var controllerComponent, out var physicsComponent, out var transformComponent))
        {
            UpdateEntity((uid, controllerComponent, physicsComponent, transformComponent), frameTime);
            
            if (!_animationQuery.TryGetComponent(uid, out var animationComponent))
                continue;
            
            UpdateAnimation((uid, animationComponent, physicsComponent), controllerComponent.Grounded);
        }
    }
    
    private void OnMoveInput(Entity<AnimationPlatformerControllerComponent> entity, ref MoveInputEvent args)
    {
        var moving = (args.Entity.Comp.HeldButtons & PlatformerControllerButtons.AnyDirection) != PlatformerControllerButtons.None;
        
        if (!_spriteQuery.TryGetComponent(entity, out var spriteComponent))
            return;

        var state = moving ? "run" : "idle";
        SetState((entity, entity, spriteComponent), state);
    }
    
    private void OnGrounded(Entity<AnimationPlatformerControllerComponent> entity, ref GroundedPlatformerControllerEvent args)
    {
        if (!_spriteQuery.TryGetComponent(entity, out var spriteComponent))
            return;
        
        if (entity.Comp.State is "run" or "idle")
            return;
        
        SetState((entity, entity, spriteComponent), "idle");
    }

    private void UpdateAnimation(Entity<AnimationPlatformerControllerComponent, PhysicsComponent> entity, bool grounded)
    {
        if (!_spriteQuery.TryGetComponent(entity, out var spriteComponent))
            return;
            
        var physicsComponent = entity.Comp2;
        var velocity = physicsComponent.LinearVelocity;

        var scale = Math.Sign(velocity.X);
        if (scale != 0)
            SetScale((entity, entity, spriteComponent), spriteComponent.Scale with { X = scale });
        
        if (grounded)
            return;
    
        if (velocity.Y > 0.1f)
            SetState((entity, entity, spriteComponent), "jump");
        
        if (velocity.Y < 0.1f)
            SetState((entity, entity, spriteComponent), "fall");
    }
    
    private void SetState(Entity<AnimationPlatformerControllerComponent, SpriteComponent> entity, string state)
    {
        var controllerComponent = entity.Comp1;
        var spriteComponent = entity.Comp2;
        
        if (controllerComponent.State == state)
            return;

        controllerComponent.State = state;
        foreach (var layer in controllerComponent.Layers)
        {
            spriteComponent.LayerSetState(layer, state);
        }
    }

    private void SetScale(Entity<AnimationPlatformerControllerComponent, SpriteComponent> entity, Vector2 vector)
    {
        var controllerComponent = entity.Comp1;
        var spriteComponent = entity.Comp2;
        
        foreach (var layer in controllerComponent.Layers)
        {
            spriteComponent.LayerSetScale(layer, vector);
        }
    }
}