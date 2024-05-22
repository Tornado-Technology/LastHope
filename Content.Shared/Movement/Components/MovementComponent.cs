using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Timing;

namespace Content.Shared.Movement.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MovementComponent : Component
{
    [ViewVariables]
    public bool Sprinting => HeldMoveButtons.HasFlag(MoveButtons.Sprint);
    
    /// <summary>
    /// The current value of the pressed buttons,
    /// can store multiple values thanks to bit shifting,
    /// to understand which bit is responsible for which button see <see cref="MoveButtons"/>.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public MoveButtons HeldMoveButtons = MoveButtons.None;
    
    [ViewVariables]
    public GameTick LastInputTick;
    
    [ViewVariables]
    public ushort LastInputSubTick;
    
    [ViewVariables]
    public Vector2 CurTickWalkMovement;
    
    [ViewVariables]
    public Vector2 CurTickSprintMovement;
}