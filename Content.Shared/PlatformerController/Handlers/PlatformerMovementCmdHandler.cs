using Content.Shared.PlatformerController.Flags;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;

namespace Content.Shared.PlatformerController.Handlers;

public sealed class PlatformerMovementCmdHandler : InputCmdHandler
{
    private readonly SharedPlatformerController _controller;
    private readonly PlatformerControllerButtons _button;
    
    public PlatformerMovementCmdHandler(SharedPlatformerController controller, PlatformerControllerButtons button)
    {
        _controller = controller;
        _button = button;
    }

    public override bool HandleCmdMessage(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
    {
        if (session?.AttachedEntity is not { } entity) 
            return false;
        
        _controller.HandleMovement(entity, _button, message.SubTick, message.State == BoundKeyState.Down);
        return false;
    }
}