namespace Content.Client.Gameplay;

public abstract class GameplayStateBase : Robust.Client.State.State, IEntityEventSubscriber
{
    protected override void Startup()
    {
    }

    protected override void Shutdown()
    {
    }
}