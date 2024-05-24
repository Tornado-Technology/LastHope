using Content.Client.States.Main.UI;
using Content.Client.UIControllers.MainMenu;
using Content.Shared.Utilities;
using Robust.Client;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.States.Main;

public sealed class MainScreenState : Robust.Client.State.State
{
    [Dependency] private readonly IBaseClient _client = default!;
    [Dependency] private readonly IGameController _controllerProxy = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;

    private ISawmill _sawmill = default!;
    
    private MainScreenControl _mainScreen = default!;
    private MultiplayerUIController _multiplayerUiController = default!;
    
    protected override void Startup()
    {
        _sawmill = _log.GetSawmill(nameof(MainScreenState));
        
        _mainScreen = new MainScreenControl();
        _userInterface.StateRoot.AddChild(_mainScreen);

        _multiplayerUiController = _userInterface.GetUIController<MultiplayerUIController>();
        _multiplayerUiController.OnConnect += TryConnect;
        
        _mainScreen.SingleplayerButton.OnPressed += SingleplayerButtonPressed;
        _mainScreen.MultiplayerButton.OnPressed += MultiplayerButtonPressed;
        _mainScreen.OptionsButton.OnPressed += OptionsButtonPressed;
        _mainScreen.QuitButton.OnPressed += QuitButtonPressed;
    }

    protected override void Shutdown()
    {
        _mainScreen.Dispose();
        _multiplayerUiController.CloseWindow();
    }

    private void TryConnect(string address, string username)
    {
        // Remove spaces
        username = username.Trim();
        
        // TODO: Username validation
        
        try
        {
            Address.Parse(address, out var ip, out var port, _client.DefaultPort);
            
            _client.ConnectToServer(ip, port);
        }
        catch (Exception exception)
        {
            _userInterface.Popup($"Unable to connect: {exception.Message}", "Connection error.");
            _sawmill.Warning(exception.ToString());
            throw;
        }
    }
    
    private void SingleplayerButtonPressed(BaseButton.ButtonEventArgs obj)
    {
        
    }
    
    private void MultiplayerButtonPressed(BaseButton.ButtonEventArgs obj)
    {
        _multiplayerUiController.ToggleWindow();
    }
    
    private void OptionsButtonPressed(BaseButton.ButtonEventArgs obj)
    {

    }
    
    private void QuitButtonPressed(BaseButton.ButtonEventArgs args)
    {
        _controllerProxy.Shutdown();
    }
}