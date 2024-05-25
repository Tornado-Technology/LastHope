using Robust.Shared.Prototypes;

namespace Content.Shared.Tag;

/// <summary>
/// A prototype storing only tag identifiers
/// is used to check the tag for existence,
/// to remove errors due to typos.
/// </summary>
[Prototype("tag")]
public sealed class TagPrototype : IPrototype
{
    [IdDataField, ViewVariables]
    public string ID { get; } = string.Empty;
}