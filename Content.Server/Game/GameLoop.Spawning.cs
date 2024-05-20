using Content.Shared.Game.Events;
using Robust.Shared.Player;

namespace Content.Server.Game;

public sealed partial class GameLoop
{
    private void InitializeSpawning()
    {
        
    }
    
    public void SpawnPlayer(ICommonSession session)
    {
        _sawmill.Debug($"Spawning {session.Name}");
        
        var client = session.Channel;
        RaiseNetworkEvent(new TickerJoinGameEvent(), client);
    }
}
