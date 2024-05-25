using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Tag;

public sealed class TagSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    
    private EntityQuery<TagComponent> _tagQuery;
    
    public override void Initialize()
    {
        base.Initialize();
        
        _tagQuery = GetEntityQuery<TagComponent>();
        
#if DEBUG
        SubscribeLocalEvent<TagComponent, ComponentInit>(OnTagInit);
#endif
    }
    
#if DEBUG
    /// <summary>
    /// Found only in DEBUG builds, so as not to overload the game in release,
    /// checks all component tags for validity.
    /// </summary>
    private void OnTagInit(Entity<TagComponent> entity, ref ComponentInit args)
    {
        foreach (var tag in entity.Comp.Tags)
        {
            ValidateTag(tag);
        }
    }
#endif

    public void AddTag(EntityUid entityUid, ProtoId<TagPrototype> tag)
    {
        AddTag((entityUid, EnsureComp<TagComponent>(entityUid)), tag);
    }

    public void AddTag(Entity<TagComponent> entity, ProtoId<TagPrototype> tag)
    {
#if DEBUG
        ValidateTag(tag);
#endif
        
        if (!entity.Comp.Tags.Add(tag))
            return;
        
        Dirty(entity);
    }

    public void AddTags(EntityUid entityUid, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        AddTags((entityUid, EnsureComp<TagComponent>(entityUid)), tags);
    }

    public void AddTags(Entity<TagComponent> entity, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        var update = false;
        foreach (var tag in tags)
        {
#if DEBUG
            ValidateTag(tag);
#endif
            if (!entity.Comp.Tags.Add(tag)) continue;
            
            update = true;
        }
        
        if (!update)
            return;
        
        Dirty(entity);
    }
    
    public bool HasTag(EntityUid entityUid, ProtoId<TagPrototype> tag)
    {
        return _tagQuery.TryGetComponent(entityUid, out var tagComponent) &&
               HasTag((entityUid, tagComponent), tag);
    }

    public bool HasTag(Entity<TagComponent> entity, ProtoId<TagPrototype> tag)
    {
#if DEBUG
        ValidateTag(tag);
#endif
        return entity.Comp.Tags.Contains(tag);
    }

    public bool HasAllTags(EntityUid entityUid, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        return _tagQuery.TryGetComponent(entityUid, out var tagComponent) &&
               HasAllTags((entityUid, tagComponent), tags);
    }
    
    public bool HasAllTags(Entity<TagComponent> entity, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        foreach (var tag in tags)
        {
#if DEBUG
            ValidateTag(tag);
#endif
            if (entity.Comp.Tags.Contains(tag))
                continue;

            return false;
        }

        return true;
    }
    
    public bool HasAnyTags(EntityUid entityUid, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        return _tagQuery.TryGetComponent(entityUid, out var tagComponent) &&
               HasAnyTags((entityUid, tagComponent), tags);
    }
    
    public bool HasAnyTags(Entity<TagComponent> entity, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        foreach (var tag in tags)
        {
#if DEBUG
            ValidateTag(tag);
#endif
            if (!entity.Comp.Tags.Contains(tag))
                continue;

            return true;
        }

        return false;
    }

    public void RemoveTag(EntityUid entityUid, ProtoId<TagPrototype> tag)
    {
        if (!_tagQuery.TryGetComponent(entityUid, out var tagComponent))
            return;
        
        RemoveTag((entityUid, tagComponent), tag);
    }
    
    public void RemoveTag(Entity<TagComponent> entity, ProtoId<TagPrototype> tag)
    {
#if DEBUG
        ValidateTag(tag);
#endif
        
        if (!entity.Comp.Tags.Remove(tag))
            return;
        
        Dirty(entity);
    }

    public void RemoveTags(EntityUid entityUid, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        if (!_tagQuery.TryGetComponent(entityUid, out var tagComponent))
            return;
        
        RemoveTags((entityUid, tagComponent), tags);
    }
    
    public void RemoveTags(Entity<TagComponent> entity, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        var update = false;
        foreach (var tag in tags)
        {
#if DEBUG
            ValidateTag(tag);
#endif
            if (!entity.Comp.Tags.Contains(tag))
                continue;

            update = true;
        }
        
        if (!update)
            return;
        
        Dirty(entity);
    }
    
    private void ValidateTag(string id)
    {
        DebugTools.Assert(_prototype.HasIndex<TagPrototype>(id), $"Unknown tag: {id}");
    }
}