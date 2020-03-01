using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.MonoGame
{
    public static class GameTimeExtensions
    {
        public static float GetDeltaTime(this GameTime time) =>
            (float)time.ElapsedGameTime.TotalSeconds;
    }
}
