using Content.Server.Game;
using Robust.Shared.Console;

namespace Content.Server.Commands;

public sealed class JoinGameCommand : LocalizedCommands
{
    private const string Name = "join_game";

    public override string Command => Name;
    public override string Description =>
        "Allows you to join the game if you are in the lobby, the \"Join\" button also calls this command.";
    public override string Help => $"{Name}";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } session)
        {
            shell.WriteError("Player session not found");
            return;
        }
        
        var entityManager = IoCManager.Resolve<IEntityManager>();
        var gameLoop = entityManager.System<GameLoop>();

        gameLoop.PlayerJoinGame(session);
    }
}