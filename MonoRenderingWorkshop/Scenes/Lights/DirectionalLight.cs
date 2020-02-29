using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class DirectionalLight : Light
    {
        private readonly Vector3 _direction;

        public DirectionalLight(Vector3 direction, float intensity, Color colour) : base(colour, intensity)
        {
            _direction = direction;
        }

        public override RenderLight GetRenderData() =>
            new RenderLight
            {
                Intensity = Intensity,
                Colour = Colour,
                ConeAngle = MathHelper.Pi,
                Direction = _direction,
                Position = Vector3.Zero
            };
    }
}