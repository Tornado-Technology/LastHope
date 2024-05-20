using Content.Shared.Game;

namespace Content.Server.Game;

public sealed partial class GameLoop : SharedGameLoop
{
    [Dependency] private readonly ILogManager _log = default!;

    private ISawmill _sawmill = default!;
    
    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _log.GetSawmill(SawmillName);
        
        InitializePlayer();
        InitializeSpawning();
    }
}