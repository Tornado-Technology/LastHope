using Content.Client.UIControllers.EntitySpawning.UI;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Enums;

namespace Content.Client.UIControllers.EntitySpawning;

public sealed partial class EntitySpawningUIController
{
    private void WindowClosed()
    {
        if (Window.SelectedButton is not null)
        {
            Window.SelectedButton.ActualButton.Pressed = false;
            Window.SelectedButton = null;
        }
        
        _placement.Clear();
    }

    private void WindowResized()
    {
        UpdateVisiblePrototypes();
    }

    private void ReplaceToggled(BaseButton.ButtonToggledEventArgs args)
    {
        if (args.Pressed)
            _placement.Replacement ^= true;

        args.Button.Pressed = args.Pressed;
    }

    private void EraseToggled(BaseButton.ButtonToggledEventArgs args)
    {
        _placement.Clear();
        
        if (args.Pressed)
            _placement.ToggleEraser();

        args.Button.Pressed = args.Pressed;
        Window.OverrideMenu.Pressed = args.Pressed;
    }

    private void ClearPressed(BaseButton.ButtonEventArgs args)
    {
        _placement.Clear();
        Window.SearchBar.Clear();
        
        BuildEntityList();
    }

    private void SearchTextChanged(LineEdit.LineEditEventArgs args)
    {
        _placement.Clear();
        
        BuildEntityList(args.Text);
        Window.ClearButton.Disabled = string.IsNullOrEmpty(args.Text);
    }

    private void OverrideSelected(OptionButton.ItemSelectedEventArgs args)
    {
        Window.OverrideMenu.SelectId(args.Id);

        if (_placement.CurrentMode is null)
            return;
        
        var newObjInfo = new PlacementInformation
        {
            PlacementOption = EntitySpawnWindow.InitOpts[args.Id],
            EntityType = _placement.CurrentPermission!.EntityType,
            Range = 2,
            IsTile = _placement.CurrentPermission.IsTile
        };

        _placement.Clear();
        _placement.BeginPlacing(newObjInfo);
    }

    private void ContainerScrolled()
    {
        UpdateVisiblePrototypes();
    }
}