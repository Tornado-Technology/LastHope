using Robust.Client.Input;
using Robust.Shared.Console;

namespace Content.Client.Commands;

public sealed class InputContextCommand : LocalizedCommands
{
    public override string Command => "set_input_context";
    public override string Help => $"{Command} <context>";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError($"Argument count must be: 1\n{Help}");
            return;
        }

        var inputManager = IoCManager.Resolve<IInputManager>();
        inputManager.Contexts.SetActiveContext(args[0]);
    }
}