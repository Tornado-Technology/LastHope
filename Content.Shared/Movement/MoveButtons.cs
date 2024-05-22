using Robust.Shared.Serialization;

namespace Content.Shared.Movement;

[Flags, Serializable, NetSerializable]
public enum MoveButtons : byte
{
    None = 0,
    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
    Sprint = 1 << 4,
    AnyDirection = Up | Down | Left | Right,
}
