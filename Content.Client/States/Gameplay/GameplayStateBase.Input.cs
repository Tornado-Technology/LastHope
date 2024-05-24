using Robust.Client.GameObjects;
using Robust.Client.Input;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Input;
using Robust.Shared.Map;

namespace Content.Client.States.Gameplay;

public abstract partial class GameplayStateBase
{
    // TODO: Rewrite this shit, when merge: https://github.com/space-wizards/RobustToolbox/pull/5157
    private void OnKeyBindStateChanged(ViewportBoundKeyEventArgs args)
    {
        // If there is no InputSystem, then there is nothing to forward to, and nothing to do here.
        if (!_entitySystemManager.TryGetEntitySystem(out InputSystem? inputSys))
            return;

        var kArgs = args.KeyEventArgs;
        var func = kArgs.Function;
        var funcId = _inputManager.NetworkBindMap.KeyFunctionID(func);

        EntityCoordinates coordinates = default;
        EntityUid? entityToClick = null;
        if (args.Viewport is IViewportControl vp)
        {
            var mousePosWorld = vp.PixelToMap(kArgs.PointerLocation.Position);
            entityToClick = GetClickedEntity(mousePosWorld);

            coordinates = _mapManager.TryFindGridAt(mousePosWorld, out _, out var grid) ?
                grid.MapToGrid(mousePosWorld) :
                EntityCoordinates.FromMap(_mapManager, mousePosWorld);
        }

        var message = new ClientFullInputCmdMessage(_timing.CurTick, _timing.TickFraction, funcId)
        {
            State = kArgs.State,
            Coordinates = coordinates,
            ScreenCoordinates = kArgs.PointerLocation,
            Uid = entityToClick ?? default,
        }; // TODO make entityUid nullable

        // client side command handlers will always be sent the local player session.
        var session = _playerManager.LocalSession;
        if (inputSys.HandleInputCommand(session, func, message))
        {
            kArgs.Handle();
        }
    }
}