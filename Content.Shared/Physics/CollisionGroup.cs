using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Serialization;

namespace Content.Shared.Physics;

[Flags, FlagsFor(typeof(CollisionLayer)), FlagsFor(typeof(CollisionMask))]
public enum CollisionGroup
{
    None = 0,
    Opaque = 1 << 0,
    Impassable = 1 << 1,
    BulletImpassable = 1 << 2,
    
    // Masks
    MobMask = Impassable,
    BlockMask = Impassable,
    
    // Layers
    MobLayer = Opaque | BulletImpassable,
    BlockLayer = Opaque | Impassable | BulletImpassable,
}