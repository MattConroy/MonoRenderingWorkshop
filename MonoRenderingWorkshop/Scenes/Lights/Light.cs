using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal abstract class Light
    {
        protected float Intensity { get; }
        protected Color Colour { get; }

        protected Light(Color colour, float intensity)
        {
            Intensity = intensity;
            Colour = colour;
        }

        public abstract void Update(GameTime time);
        public abstract RenderLight GetRenderData();
    }
}