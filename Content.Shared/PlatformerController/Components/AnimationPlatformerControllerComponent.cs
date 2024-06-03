using System.Numerics;
using Content.Shared.Humanoid;

namespace Content.Shared.PlatformerController.Components;

[RegisterComponent]
public sealed partial class AnimationPlatformerControllerComponent : Component
{
    [DataField]
    public string State = string.Empty;
    
    [DataField]
    public HashSet<HumanoidVisualLayers> Layers = new();

    [DataField]
    public Dictionary<string, Dictionary<HumanoidVisualLayers, HashSet<Vector2>>> Offsets = new();
}