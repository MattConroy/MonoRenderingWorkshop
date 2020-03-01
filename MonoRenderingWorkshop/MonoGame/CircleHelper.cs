using Microsoft.Xna.Framework;
using System;

namespace MonoRenderingWorkshop.MonoGame
{
    public static class CircleHelper
    {
        public static Vector2 GetPointOnUnitCircle(float angleInRadians) =>
            new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians));

        public static Vector2 GetPointOnUnitCircle(int index, int count) =>
            GetPointOnUnitCircle(GetAngleFromNumberOfPoints(index, count));

        public static Vector2 GetPointOnCircle(Vector2 centre, float radius, float angleInRadians) =>
            centre + GetPointOnUnitCircle(angleInRadians) * radius;

        public static Vector2 GetPointOnCircle(Vector2 centre, float radius, int index, int count) =>
            GetPointOnCircle(centre, radius, GetAngleFromNumberOfPoints(index, count));

        public static float GetAngleFromNumberOfPoints(int index, int count) =>
            index * (MathHelper.TwoPi / count);
    }
}
