using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal abstract class Light
    {
        public Vector3 Position { get; set; }
        protected LightColour Ambient { get; }
        protected LightColour Diffuse { get; }

        protected Light(LightColour ambient, LightColour diffuse)
        {
            Ambient = ambient;
            Diffuse = diffuse;
        }

        public abstract void Update(GameTime time);
        public abstract RenderLight GetRenderData();
    }
}