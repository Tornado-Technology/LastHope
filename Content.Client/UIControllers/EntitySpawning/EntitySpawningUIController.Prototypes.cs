using System.Linq;
using System.Numerics;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.UIControllers.EntitySpawning;

public sealed partial class EntitySpawningUIController
{ 
    private static bool DoesEntityMatchSearch(EntityPrototype prototype, string searchStr)
    {
        // If we haven't entered anything, we return prototype
        if (string.IsNullOrEmpty(searchStr))
            return true;

        if (prototype.ID.Contains(searchStr, StringComparison.InvariantCultureIgnoreCase))
            return true;

        if (prototype.EditorSuffix?.Contains(searchStr, StringComparison.InvariantCultureIgnoreCase) ?? false)
            return true;

        if (prototype.Name.Contains(searchStr, StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }
    
    private void BuildEntityList(string search = "")
    {
        _shownEntities.Clear();
        
        // Reset last prototype indices so it automatically updates the entire list.
        _lastEntityIndices = (0, -1);
        
        // Cleaning UI from old prototypes
        Window.PrototypeList.RemoveAllChildren();
        Window.SelectedButton = null;
        
        // Always search case-insensitive, it often just gets in the way
        search = search.ToLowerInvariant();

        foreach (var prototype in _prototypes.EnumeratePrototypes<EntityPrototype>())
        {
            if (prototype.Abstract)
                continue;

            if (prototype.HideSpawnMenu)
                continue;

            if (!string.IsNullOrEmpty(search) && !DoesEntityMatchSearch(prototype, search))
                continue;

            _shownEntities.Add(prototype);
        }

        _shownEntities.Sort((a, b) =>
        {
            var name = string.Compare(a.Name, b.Name, StringComparison.Ordinal);
            
            if (name != 0)
                return name;
            
            return string.Compare(a.EditorSuffix, b.EditorSuffix, StringComparison.Ordinal);
        });

        Window.PrototypeList.TotalItemCount = _shownEntities.Count;
        Window.PrototypeScrollContainer.SetScrollValue(Vector2.Zero);
        
        UpdateVisiblePrototypes();
    }
    
    // Update visible buttons in the prototype list.
    private void UpdateVisiblePrototypes()
    {
        // Calculate index of first prototype to render based on current scroll.
        var height = Window.MeasureButton.DesiredSize.Y + PrototypeListContainer.Separation;
        var offset = Math.Max(-Window.PrototypeList.Position.Y, 0);
        var startIndex = (int) Math.Floor(offset / height);
        
        Window.PrototypeList.ItemOffset = startIndex;

        var (prevStart, prevEnd) = _lastEntityIndices;

        // Calculate index of final one.
        var endIndex = startIndex - 1;
        var spaceUsed = -height; // -height instead of 0 because else it cuts off the last button.

        while (spaceUsed < Window.PrototypeList.Parent!.Height)
        {
            spaceUsed += height;
            endIndex += 1;
        }

        endIndex = Math.Min(endIndex, _shownEntities.Count - 1);

        if (endIndex == prevEnd && startIndex == prevStart)
        {
            // Nothing changed so bye.
            return;
        }

        _lastEntityIndices = (startIndex, endIndex);

        // Delete buttons at the start of the list that are no longer visible (scrolling down).
        for (var i = prevStart; i < startIndex && i <= prevEnd; i++)
        {
            var control = (EntitySpawnButton) Window.PrototypeList.GetChild(0);
            DebugTools.Assert(control.Index == i);
            Window.PrototypeList.RemoveChild(control);
        }

        // Delete buttons at the end of the list that are no longer visible (scrolling up).
        for (var i = prevEnd; i > endIndex && i >= prevStart; i--)
        {
            var control = (EntitySpawnButton) Window.PrototypeList.GetChild(Window.PrototypeList.ChildCount - 1);
            DebugTools.Assert(control.Index == i);
            Window.PrototypeList.RemoveChild(control);
        }

        // Create buttons at the start of the list that are now visible (scrolling up).
        for (var i = Math.Min(prevStart - 1, endIndex); i >= startIndex; i--)
        {
            InsertEntityButton(_shownEntities[i], true, i);
        }

        // Create buttons at the end of the list that are now visible (scrolling down).
        for (var i = Math.Max(prevEnd + 1, startIndex); i <= endIndex; i++)
        {
            InsertEntityButton(_shownEntities[i], false, i);
        }
    }
    
    private void InsertEntityButton(EntityPrototype prototype, bool insertFirst, int index)
    {
        var textures = SpriteComponent.GetPrototypeTextures(prototype, _resources).Select(o => o.Default).ToList();
        var button = Window.InsertEntityButton(prototype, insertFirst, index, textures);

        button.ActualButton.OnToggled += OnEntityButtonToggled;
    }

    private void OnEntityButtonToggled(BaseButton.ButtonToggledEventArgs args)
    {
        var item = (EntitySpawnButton) args.Button.Parent!;
        
        if (Window.SelectedButton == item)
        {
            Window.SelectedButton = null;
            Window.SelectedPrototype = null;
            _placement.Clear();
            return;
        }

        if (Window.SelectedButton != null)
        {
            Window.SelectedButton.ActualButton.Pressed = false;
        }

        Window.SelectedButton = null;
        Window.SelectedPrototype = null;

        var overrideMode = EntitySpawnWindow.InitOpts[Window.OverrideMenu.SelectedId];
        var newObjInfo = new PlacementInformation
        {
            PlacementOption = overrideMode != "Default" ? overrideMode : item.Prototype.PlacementMode,
            EntityType = item.PrototypeID,
            Range = 2,
            IsTile = false
        };

        _placement.BeginPlacing(newObjInfo);

        Window.SelectedButton = item;
        Window.SelectedPrototype = item.Prototype;
    }
}