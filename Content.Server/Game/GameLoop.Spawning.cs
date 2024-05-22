using System.Numerics;
using Content.Shared.Game.Events;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Server.Game;

public sealed partial class GameLoop
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    
    private void InitializeSpawning()
    {
        
    }
    
    public void SpawnPlayer(ICommonSession session)
    {
        _sawmill.Debug($"Spawning {session.Name}");

        var map = _mapManager.CreateMap();
        
        var playerEntity = Spawn("Player", new MapCoordinates(Vector2.Zero, map));
        _playerManager.SetAttachedEntity(session, playerEntity);
        
        var client = session.Channel;
        RaiseNetworkEvent(new TickerJoinGameEvent(), client);
    }
}
