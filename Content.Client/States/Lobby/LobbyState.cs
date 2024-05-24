using Content.Client.States.Lobby.UI;

namespace Content.Client.States.Lobby;

public sealed class LobbyState : Robust.Client.State.State
{
    private LobbyScreen _lobbyScreen = default!;
    
    protected override void Startup()
    {
        _lobbyScreen = new LobbyScreen();
    }

    protected override void Shutdown()
    {
        _lobbyScreen.Dispose();
    }
}