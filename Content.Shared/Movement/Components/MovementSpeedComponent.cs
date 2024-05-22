using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class MovementSpeedComponent : Component
{
    [ViewVariables]
    public const float DefaultBaseWalkSpeed = 2.5f;
    
    [ViewVariables]
    public const float DefaultBaseSprintSpeed = 4.5f;

    [ViewVariables]
    public const float DefaultAcceleration = 20f;
    
    [ViewVariables]
    public const float DefaultFriction = 20f;
    
    [ViewVariables]
    public const float DefaultFrictionNoInput = 20f;
    
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float WalkSpeed = DefaultBaseWalkSpeed;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SprintSpeed = DefaultBaseSprintSpeed;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Acceleration = DefaultAcceleration;
    
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Friction = DefaultFriction;
    
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float FrictionNoInput = DefaultFrictionNoInput;
}