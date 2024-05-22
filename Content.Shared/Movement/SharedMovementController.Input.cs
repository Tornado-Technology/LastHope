using System.Numerics;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Handlers;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;

namespace Content.Shared.Movement;

public abstract partial class SharedMovementController
{
    private void InitializeInput()
    {
        CommandBinds.Builder
            .Bind(EngineKeyFunctions.MoveLeft, new MovementHorizontalCmdHandler(this, Direction.West))
            .Bind(EngineKeyFunctions.MoveRight, new MovementHorizontalCmdHandler(this, Direction.East))
            .Register<SharedMovementController>();
    }
    
    /// <summary>
    /// Receives and processes the attempted change of direction caused in <see cref="MovementHorizontalCmdHandler"/>.
    /// </summary>
    public void HandleHorizontal(EntityUid entityUid, Direction direction, ushort subTick, bool buttonPressed)
    {
        if (!_movementQuery.TryGetComponent(entityUid, out var movementComponent))
            return;

        SetMoveInput((entityUid, movementComponent), direction, subTick, buttonPressed);
    }
    
    private void SetMoveInput(Entity<MovementComponent> movement, Direction direction, ushort subTick, bool buttonPressed)
    {
        SetMoveInput(movement, GetButtonFromDirection(direction), subTick, buttonPressed);
    }

    private void SetMoveInput(Entity<MovementComponent> movement, MoveButtons bit, ushort subTick, bool buttonPressed)
    {
        if (movement.Comp.LastInputTick <= _timing.CurTick)
        {
            movement.Comp.CurTickWalkMovement = Vector2.Zero;
            movement.Comp.LastInputSubTick = 0;
            
            movement.Comp.LastInputTick = _timing.CurTick;
        }

        if (movement.Comp.LastInputSubTick <= subTick)
        {
            var fraction = (subTick - movement.Comp.LastInputSubTick) / (float)ushort.MaxValue;
            
            ref var moveAmount = ref movement.Comp.Sprinting
                ? ref movement.Comp.CurTickSprintMovement
                : ref movement.Comp.CurTickWalkMovement;

            moveAmount += GetVectorFromButtons(movement.Comp.HeldMoveButtons);
            moveAmount *= fraction;

            movement.Comp.LastInputSubTick = subTick;
        }

        // Get the currently pressed buttons
        var buttons = movement.Comp.HeldMoveButtons;

        // If a button is pressed
        // and we don't care if it's new or not,
        // it has no bearing on the outcome
        if (buttonPressed)
        {
            // Add to the current ones using bitwise addition (OR)
            buttons |= bit;
        }
        else
        {
            // Multiply the current buttons by the inverse of the new button value
            // Example of work:
            //
            //  1010 - Up-Left, and released the Up button
            //  Up = 1000
            // ~Up = 0111
            // 
            // 1010 * 0111 = 0010 - Left
            // All we have left is the left
            // In fact, with this action we remove a bit from our number
            buttons &= ~bit;
        }
        
        SetMoveInput(movement, buttons);
    }

    private void SetMoveInput(Entity<MovementComponent> movement, MoveButtons buttons)
    {
        if (movement.Comp.HeldMoveButtons == buttons)
            return;

        movement.Comp.HeldMoveButtons = buttons;
        Dirty(movement);
    }

    private static MoveButtons GetButtonFromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.East => MoveButtons.Right,
            Direction.North => MoveButtons.Up,
            Direction.West => MoveButtons.Left,
            Direction.SouthEast => MoveButtons.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static Vector2 GetVectorFromButtons(MoveButtons buttons)
    {
        var vector = Vector2.Zero;

        vector.X -= buttons.HasFlag(MoveButtons.Left) ? 1 : 0;
        vector.X += buttons.HasFlag(MoveButtons.Right) ? 1 : 0;
        vector.Y -= buttons.HasFlag(MoveButtons.Down) ? 1 : 0;
        vector.Y += buttons.HasFlag(MoveButtons.Up) ? 1 : 0;

        if (vector.LengthSquared() > 1.0e-6)
            return vector;
            
        return vector.Normalized();
    }
}