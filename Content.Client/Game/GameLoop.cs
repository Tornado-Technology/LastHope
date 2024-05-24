using Content.Client.States.Gameplay;
using Content.Client.States.Lobby;
using Content.Shared.Game;
using Content.Shared.Game.Events;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using Robust.Shared.Network;

namespace Content.Client.Game;

public sealed partial class GameLoop : SharedGameLoop
{
    [Dependency] private readonly IClientNetManager _netManager = default!;
    [Dependency] private readonly IConsoleHost _consoleHost = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IStateManager _state = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private SessionStatus? Status => _playerManager.LocalSession?.Status;
    
    private ISawmill _sawmill = default!;
    
    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _log.GetSawmill(SawmillName);

        SubscribeNetworkEvent<TickerJoinLobbyEvent>(JoinLobby);   
        SubscribeNetworkEvent<TickerJoinGameEvent>(JoinGame);   
    }

    private void JoinLobby(TickerJoinLobbyEvent message)
    {
        _state.RequestStateChange<LobbyState>();
    }

    private void JoinGame(TickerJoinGameEvent message)
    {
        _state.RequestStateChange<GameplayState>();
    }
}