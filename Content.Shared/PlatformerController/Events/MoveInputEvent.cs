using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Flags;

namespace Content.Shared.PlatformerController.Events;

[ByRefEvent]
public readonly struct MoveInputEvent
{
    public readonly Entity<PlatformerControllerComponent> Entity;
    public readonly PlatformerControllerButtons OldMovement;

    public MoveInputEvent(Entity<PlatformerControllerComponent> entity, PlatformerControllerButtons oldMovement)
    {
        Entity = entity;
        OldMovement = oldMovement;
    }
}