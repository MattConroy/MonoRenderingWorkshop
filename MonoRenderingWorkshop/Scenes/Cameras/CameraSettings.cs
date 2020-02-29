using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Scenes.Cameras
{
    internal class CameraSettings
    {
        public float FieldOfView { get; }
        public float NearPlane { get; }
        public float FarPlane { get; }

        public CameraSettings(float fieldOfView, float nearPlane, float farPlane)
        {
            FieldOfView = fieldOfView;
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        public float GetAspectRatio(GraphicsDevice graphicsDevice) =>
            graphicsDevice.Viewport.AspectRatio;

        public static CameraSettings DefaultSettings =>
            new CameraSettings(
                MathHelper.ToRadians(50),
                0.1f,
                100f);
    }
}