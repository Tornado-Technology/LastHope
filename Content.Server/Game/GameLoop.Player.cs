using Content.Shared.Game.Events;
using JetBrains.Annotations;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;

namespace Content.Server.Game;

public sealed partial class GameLoop
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private void InitializePlayer()
    {
        _playerManager.PlayerStatusChanged += PlayerStatusChanged;
    }

    private void PlayerStatusChanged([CanBeNull] object sender, SessionStatusEventArgs args)
    {
        switch (args.NewStatus)
        {
            case SessionStatus.Connected:
                _playerManager.JoinGame(args.Session);
                break;
            
            case SessionStatus.Connecting:
                break;
            
            case SessionStatus.Disconnected:
                break;
            
            case SessionStatus.Zombie:
                break;
            
            case SessionStatus.InGame:
                PlayerJoinLobby(args.Session);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void PlayerJoinLobby(ICommonSession session)
    {
        RaiseNetworkEvent(new TickerJoinLobbyEvent(), session.Channel);
    }

    public void PlayerJoinGame(ICommonSession session)
    {
        SpawnPlayer(session);
        RaiseNetworkEvent(new TickerJoinGameEvent(), session.Channel);
    }
}