using Content.Client.Gameplay;
using Content.Shared.Game;
using Content.Shared.Game.Events;
using Robust.Client.State;

namespace Content.Client.Game;

public sealed class GameLoop : SharedGameLoop
{
    [Dependency] private readonly IStateManager _state = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;
    
    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _log.GetSawmill(SawmillName);

        SubscribeNetworkEvent<TickerJoinGameEvent>(JoinGame);   
    }
    
    private void JoinGame(TickerJoinGameEvent message)
    {
        _sawmill.Debug("Joined");
        _state.RequestStateChange<GameplayState>();
    }
}