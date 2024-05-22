using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Timing;

namespace Content.Shared.Movement.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MovementComponent : Component
{
    public bool Sprinting => HeldMoveButtons.HasFlag(MoveButtons.Sprint);
    
    /// <summary>
    /// The current value of the pressed buttons,
    /// can store multiple values thanks to bit shifting,
    /// to understand which bit is responsible for which button see <see cref="MoveButtons"/>.
    /// </summary>
    public MoveButtons HeldMoveButtons = MoveButtons.None;
    
    public GameTick LastInputTick;
    public ushort LastInputSubTick;
    
    public Vector2 CurTickWalkMovement;
    public Vector2 CurTickSprintMovement;
}