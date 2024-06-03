using Robust.Shared.Serialization;

namespace Content.Shared.Humanoid;

[Serializable, NetSerializable]
public enum HumanoidVisualLayers : byte
{
    FArm,
    Head,
    Body,
    Hair,
    BArm,
    Tail
}