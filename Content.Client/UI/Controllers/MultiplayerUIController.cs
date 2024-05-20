using Robust.Client.UserInterface.Controllers;

namespace Content.Client.UI.Controllers;

public sealed class MultiplayerUiController : UIController
{
    private MultiplayerMenu _window = default!;

    public Action<string, string>? OnConnect;
    
    public void ToggleWindow()
    {
        EnsureWindow();
        
        if (_window.IsOpen)
        {
            CloseWindow();
            return;
        }

        OpenWindow();
    }

    private void OpenWindow()
    {
        EnsureWindow();
            
        _window.OpenCentered();
        _window.MoveToFront();
    }

    public void CloseWindow()
    {
        EnsureWindow();
        
        _window.Close();
    }
    
    private void EnsureWindow()
    {
        if (_window is { Disposed: false })
            return;

        _window = UIManager.CreateWindow<MultiplayerMenu>();
        _window.DirectConnectButton.OnPressed += _ =>
        {
            OnConnect?.Invoke(
                _window.AddressBox.Text,
                _window.UsernameBox.Text);
        };
    }
}