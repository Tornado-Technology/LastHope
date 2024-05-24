using Content.Client.Game;
using Content.Client.States.Lobby.UI;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.States.Lobby;

public sealed class LobbyState : Robust.Client.State.State
{
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    
    private GameLoop _game = default!;
    
    private LobbyScreen _lobbyScreen = default!;
    
    protected override void Startup()
    {
        _game = _entityManager.System<GameLoop>();
        
        _lobbyScreen = new LobbyScreen();
        _userInterface.StateRoot.AddChild(_lobbyScreen);

        _lobbyScreen.JoinButton.OnPressed += JoinGame;
        _lobbyScreen.LeaveButton.OnPressed += Leave;
    }

    protected override void Shutdown()
    {
        _lobbyScreen.Dispose();
    }
    
    private void JoinGame(BaseButton.ButtonEventArgs args)
    {
        _game.JoinGame();
    }
    
    private void Leave(BaseButton.ButtonEventArgs args)
    {
        _game.Leave();
    }
}