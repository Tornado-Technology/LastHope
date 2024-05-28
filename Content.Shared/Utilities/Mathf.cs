using System.Numerics;

namespace Content.Shared.Utilities;

public static class Mathf
{
    public static float MoveTowards(float current, float target, float delta)
    {
        return current < target ? Math.Min(current + delta, target) : Math.Max(current - delta, target);
    }
    
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float delta)
    {
        return new Vector2(MoveTowards(current.X, target.X, delta), MoveTowards(current.Y, target.Y, delta));
    }
}