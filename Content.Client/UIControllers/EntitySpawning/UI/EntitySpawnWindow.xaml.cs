﻿using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;

namespace Content.Client.UIControllers.EntitySpawning.UI;

[GenerateTypedNameReferences]
public sealed partial class EntitySpawnWindow : DefaultWindow
{ 
    public static readonly string[] InitOpts =
    {
         "Default",
         "PlaceFree",
         "PlaceNearby",
         "SnapgridCenter",
         "SnapgridBorder",
         "AlignSimilar",
         "AlignTileAny",
         "AlignTileEmpty",
         "AlignTileNonDense",
         "AlignTileDense",
         "AlignWall",
         "AlignWallProper",
    };

     public EntitySpawnButton? SelectedButton;
     public EntityPrototype? SelectedPrototype;

     public EntitySpawnWindow()
     {
         RobustXamlLoader.Load(this);

         MeasureButton.Measure(Vector2Helpers.Infinity);

         for (var i = 0; i < InitOpts.Length; i++)
         {
             OverrideMenu.AddItem(InitOpts[i], i);
         }
     }

     // Create a spawn button and insert it into the start or end of the list.
     public EntitySpawnButton InsertEntityButton(EntityPrototype prototype, bool insertFirst, int index, List<Texture> textures)
     {
         var button = new EntitySpawnButton
         {
             Prototype = prototype,
             Index = index // We track this index purely for debugging.
         };
         var entityLabelText = string.IsNullOrEmpty(prototype.Name) ? prototype.ID : prototype.Name;

         if (!string.IsNullOrWhiteSpace(prototype.EditorSuffix))
         {
             entityLabelText += $" [{prototype.EditorSuffix}]";
         }

         button.EntityLabel.Text = entityLabelText;

         if (prototype == SelectedPrototype)
         {
             SelectedButton = button;
             SelectedButton.ActualButton.Pressed = true;
         }

         var rect = button.EntityTextureRects;
         rect.Textures = textures;

         PrototypeList.AddChild(button);
         if (insertFirst)
         {
             button.SetPositionInParent(0);
         }

         return button;
    }
}