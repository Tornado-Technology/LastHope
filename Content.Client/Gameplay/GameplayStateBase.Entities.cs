using System.Numerics;
using Robust.Client.ComponentTrees;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Map;

namespace Content.Client.Gameplay;

public abstract partial class GameplayStateBase
{
    private EntityUid? RecursivelyFindUiEntity(Control? control)
    {
        if (control == null)
            return null;

        switch (control)
        {
            case IViewportControl vp:
                return _inputManager.MouseScreenPosition.IsValid ? GetClickedEntity(vp.PixelToMap(_inputManager.MouseScreenPosition.Position)) : null;

            case SpriteView sprite:
                return sprite.Entity;
            
            default:
                return RecursivelyFindUiEntity(control.Parent);
        }
    }
    
    public IEnumerable<EntityUid> GetClickableEntities(EntityCoordinates coordinates)
    {
        return GetClickableEntities(coordinates.ToMap(_entityManager, _entitySystemManager.GetEntitySystem<SharedTransformSystem>()));
    }

    public IEnumerable<EntityUid> GetClickableEntities(MapCoordinates coordinates)
    {
        // Find all the entities intersecting our click
        var spriteTree = _entityManager.EntitySysManager.GetEntitySystem<SpriteTreeSystem>();
        var entities = spriteTree.QueryAabb(coordinates.MapId, Box2.CenteredAround(coordinates.Position, new Vector2(1, 1)));

        // Check the entities against whether or not we can click them
        var foundEntities = new List<EntityUid>(entities.Count);

        foreach (var entity in entities)
        {
            foundEntities.Add(entity.Uid);
        }
        
        return foundEntities;
    }
}