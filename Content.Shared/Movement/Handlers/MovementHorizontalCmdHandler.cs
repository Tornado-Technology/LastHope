using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;

namespace Content.Shared.Movement.Handlers;

public sealed class MovementHorizontalCmdHandler : InputCmdHandler
{
    private readonly SharedMovementController _controller;
    private readonly Direction _direction;
    
    public MovementHorizontalCmdHandler(SharedMovementController controller, Direction direction)
    {
        _controller = controller;
        _direction = direction;
    }

    public override bool HandleCmdMessage(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
    {
        if (session?.AttachedEntity is not { } entity) 
            return false;
        
        _controller.HandleHorizontal(entity, _direction, message.SubTick, message.State == BoundKeyState.Down);
        return false;
    }
}