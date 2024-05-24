using Robust.Shared.Enums;

namespace Content.Client.Game;

public sealed partial class GameLoop
{
    public void JoinGame()
    {
        if (Status != SessionStatus.InGame)
        {
            Log.Error($"Attempting to join the game world, not in game status {Status}");
            return;
        }
        
        // TODO: Maybe realize with netMessage?
        _consoleHost.ExecuteCommand("join_game");
    }

    public void Leave()
    {
        // TODO: Add reason
        _netManager.ClientDisconnect(string.Empty);
    }
}