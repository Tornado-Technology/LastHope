using Robust.Shared.Serialization;

using DrawDepthTag = Robust.Shared.GameObjects.DrawDepth;

namespace Content.Shared.DrawDepth;

[ConstantsFor(typeof(DrawDepthTag))]
public enum DrawDepth
{
    Blocks = DrawDepthTag.Default - 1,
    
    Objects = DrawDepthTag.Default,
    
    Items = DrawDepthTag.Default + 1,

    Mobs = DrawDepthTag.Default + 2,
}