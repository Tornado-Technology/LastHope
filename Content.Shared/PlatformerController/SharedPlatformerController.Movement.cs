using System.Numerics;
using System.Runtime.ExceptionServices;
using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Events;
using Content.Shared.PlatformerController.Flags;

namespace Content.Shared.PlatformerController;

public abstract partial class SharedPlatformerController
{
    private void InitializeMovement()
    {

    }
    
    private void HandleMovement(Entity<PlatformerControllerComponent> entity, PlatformerControllerButtons bit, ushort subTick, bool buttonPressed)
    {
        var buttons = entity.Comp.HeldButtons;
        if (buttonPressed)
        {
            buttons |= bit;
        }
        else
        {
            buttons &= ~bit;
        }
        
        SetMoveInput(entity, buttons);
    }

    private void SetMoveInput(Entity<PlatformerControllerComponent> entity, PlatformerControllerButtons buttons)
    {
        if (entity.Comp.HeldButtons == buttons)
            return;
        
        entity.Comp.HeldButtons = buttons;
        entity.Comp.Movement = GetVector(buttons);

        var ev = new MoveInputEvent(entity, entity.Comp.HeldButtons);
        RaiseLocalEvent(entity, ref ev);
    }

    private static Vector2 GetVector(PlatformerControllerButtons buttons)
    {
        var vector = new Vector2();
        
        vector.X -= buttons.HasFlag(PlatformerControllerButtons.Left) ? 1 : 0;
        vector.X += buttons.HasFlag(PlatformerControllerButtons.Right) ? 1 : 0;
        vector.Y -= buttons.HasFlag(PlatformerControllerButtons.Down) ? 1 : 0;
        vector.Y += buttons.HasFlag(PlatformerControllerButtons.Up) ? 1 : 0;

        return vector;
    }
}