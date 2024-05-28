using Content.Shared.PlatformerController;
using Content.Shared.PlatformerController.Components;
using Robust.Shared.Physics.Components;

namespace Content.Server.PlatformerController;

public sealed class PlatformerController : SharedPlatformerController
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<PlatformerControllerComponent, PhysicsComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var controllerComponent, out var physicsComponent, out var transformComponent))
        {
            UpdateEntity((uid, controllerComponent, physicsComponent, transformComponent), frameTime);
        }
    }
}