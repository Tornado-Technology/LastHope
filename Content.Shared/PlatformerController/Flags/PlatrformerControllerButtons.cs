using Robust.Shared.Serialization;

namespace Content.Shared.PlatformerController.Flags;

[Flags, Serializable, NetSerializable]
public enum PlatformerControllerButtons : byte
{
    None  = 0,
    // Directions
    Up    = 1 << 0,
    Down  = 1 << 1,
    Left  = 1 << 2,
    Right = 1 << 3,
    AnyDirection = Up | Down | Left | Right,
    // Jump
    Jump  = 1 << 4
}