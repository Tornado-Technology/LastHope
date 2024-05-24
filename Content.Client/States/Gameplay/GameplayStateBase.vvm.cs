namespace Content.Client.States.Gameplay;

public abstract partial class GameplayStateBase
{
    private (ViewVariablesPath? path, string[] segments) ResolveVvHoverObject(string path)
    {
        var segments = path.Split('/');
        var uid = RecursivelyFindUiEntity(_userInterface.CurrentlyHovered);
        var netUid = _entityManager.GetNetEntity(uid);
        return (netUid != null ? new ViewVariablesInstancePath(netUid) : null, segments);
    }
    
    private IEnumerable<string>? ListVvHoverPaths(string[] segments)
    {
        return null;
    }
}
