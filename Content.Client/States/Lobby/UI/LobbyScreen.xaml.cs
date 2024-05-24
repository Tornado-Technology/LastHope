﻿using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.States.Lobby.UI;

[GenerateTypedNameReferences]
public sealed partial class LobbyScreen : UIScreen
{
    public LobbyScreen()
    {
        RobustXamlLoader.Load(this);
    }
}