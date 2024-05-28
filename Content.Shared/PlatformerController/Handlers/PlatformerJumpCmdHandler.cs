using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;

namespace Content.Shared.PlatformerController.Handlers;

public sealed class PlatformerJumpCmdHandler : InputCmdHandler
{
    private readonly SharedPlatformerController _controller;

    public PlatformerJumpCmdHandler(SharedPlatformerController controller)
    {
        _controller = controller;
    }
    
    public override bool HandleCmdMessage(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
    {
        if (session?.AttachedEntity is not { } entity) 
            return false;

        _controller.HandleJump(entity, message.SubTick, message.State == BoundKeyState.Down);
        return false;
    }
}