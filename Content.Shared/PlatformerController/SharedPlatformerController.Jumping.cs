using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Flags;

namespace Content.Shared.PlatformerController;

public abstract partial class SharedPlatformerController
{
    private void InitializeJumping()
    {
        
    }
    
    private void HandleJump(Entity<PlatformerControllerComponent> entity, ushort subTick, bool buttonPressed)
    {
        var buttons = entity.Comp.HeldButtons;
        if (buttonPressed)
        {
            buttons |= PlatformerControllerButtons.Jump;
        }
        else
        {
            buttons &= ~PlatformerControllerButtons.Jump;
        }
        
        SetJumpInput(entity, buttons);
    }

    private void SetJumpInput(Entity<PlatformerControllerComponent> entity, PlatformerControllerButtons buttons)
    {       
        entity.Comp.HeldButtons = buttons;
    }
}