using System.Numerics;
using Robust.Client.Graphics;
using Robust.Shared.Console;

namespace Content.Client.Commands;

public sealed class ZoomCommand : LocalizedCommands
{
    private const int ArgumentNumber = 1;
    
    [Dependency] private readonly IEyeManager _eyeManager = default!;
    
    public override string Command => "zoom";
    public override string Help => "zoom <value>";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != ArgumentNumber)
        {
            shell.WriteError($"Argument count must be: {ArgumentNumber}\n{Help}");
            return;
        }
        
        if (!float.TryParse(args[0], out var value))
        {
            shell.WriteError(LocalizationManager.GetString("cmd-parse-failure-float", ("arg", args[1])));
            return;
        }

        _eyeManager.CurrentEye.Zoom = new Vector2(value);
    }
}