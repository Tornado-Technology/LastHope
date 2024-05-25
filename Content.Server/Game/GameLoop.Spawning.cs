using System.Numerics;
using Content.Shared.Game.Events;
using Robust.Shared.Map;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Player;

namespace Content.Server.Game;

public sealed partial class GameLoop
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly Gravity2DController _gravity = default!;

    private MapId TestMap
    {
        get
        {
            if (_map is not null)
                return _map.Value;
            
            _map = _mapManager.CreateMap();
            _gravity.SetGravity(_map.Value, new Vector2(0, -9.8f));

            return _map.Value;
        }
    }

    private MapId? _map;
    
    private void InitializeSpawning()
    {
    }
    
    public void SpawnPlayer(ICommonSession session)
    {
        _sawmill.Debug($"Spawning {session.Name}");
        
        var playerEntity = Spawn("Player", new MapCoordinates(Vector2.Zero, TestMap));
        _playerManager.SetAttachedEntity(session, playerEntity);
    }
}
