using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared;

[CVarDefs]
public sealed class GameConfigVars : CVars
{
    public static readonly CVarDef<bool> HudFpsCounterVisible =
            CVarDef.Create("hud.fps_counter_visible", true, CVar.CLIENTONLY | CVar.ARCHIVE);
}