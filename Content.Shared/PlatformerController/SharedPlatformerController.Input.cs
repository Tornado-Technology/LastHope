using Content.Shared.Input;
using Content.Shared.PlatformerController.Components;
using Content.Shared.PlatformerController.Flags;
using Content.Shared.PlatformerController.Handlers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;

namespace Content.Shared.PlatformerController;

public abstract partial class SharedPlatformerController
{
    private void InitializeInput()
    {
        CommandBinds.Builder
            // Movement
            .Bind(EngineKeyFunctions.MoveLeft, new PlatformerMovementCmdHandler(this, PlatformerControllerButtons.Left))
            .Bind(EngineKeyFunctions.MoveRight, new PlatformerMovementCmdHandler(this, PlatformerControllerButtons.Right))
            // Jumping
            .Bind(ContentKeyFunctions.Jump, new PlatformerJumpCmdHandler(this))
            .Register<SharedPlatformerController>();
    }

    public void HandleMovement(EntityUid entityUid, PlatformerControllerButtons button, ushort subTick, bool buttonPressed)
    {
        if (!_platformerControllerQuery.TryGetComponent(entityUid, out var component))
            return;
        
        HandleMovement((entityUid, component), button, subTick, buttonPressed);
    }
    
    public void HandleJump(EntityUid entityUid, ushort subTick, bool buttonPressed)
    {
        if (!_platformerControllerQuery.TryGetComponent(entityUid, out var component))
            return;
        
        HandleJump((entityUid, component), subTick, buttonPressed);   
    }
}