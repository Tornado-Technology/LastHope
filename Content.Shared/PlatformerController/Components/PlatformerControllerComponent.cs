using System.Numerics;
using Content.Shared.PlatformerController.Flags;
using Robust.Shared.GameStates;

namespace Content.Shared.PlatformerController.Components;

/// <summary>
/// The component responsible for storing data shared between other controllers components.
/// Without it, other components of the controller will not work,
/// so you need to have this component on the entity for them to work.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PlatformerControllerComponent : Component
{
    [ViewVariables]
    public Vector2 Movement;

    /// <summary>
    /// Requires additional synchronization that the animation would work on all clients correctly,
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public PlatformerControllerButtons HeldButtons;

    [ViewVariables]
    public bool EndedJumpEarly;
    
    [ViewVariables, AutoNetworkedField]
    public bool Grounded;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float JumpEndEarlyGravityModifier = 0.7f;
    
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float JumpForce = 10;
    
    /// <summary>
    /// The speed of the entity gaining speed.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float Acceleration = 120;

    /// <summary>
    /// The coefficient by which a player slows down when
    /// touching a surface is the coefficient of the player himself,
    /// regardless of the parameters of the surface
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float Friction = 120;

    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float AirFriction = 30;
    
    /// <summary>
    /// The maximum speed to which the entity can accelerate.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float MaxSpeed = 14;

    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float MaxFallSpeed = 40;
}