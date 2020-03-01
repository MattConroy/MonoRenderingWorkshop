using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class DirectionalLight : Light
    {
        private readonly Vector3 _direction;

        public DirectionalLight(Vector3 direction, Color colour, float intensity) : base(colour, intensity)
        {
            _direction = direction;
            _direction.Normalize();
        }

        public override void Update(GameTime time)
        {
        }

        public override RenderLight GetRenderData() =>
            new RenderLight
            {
                Colour = Colour,
                Intensity = Intensity,
                Direction = _direction,
                DirectionFactor = 1,
                Position = Vector3.Zero,
                Attenuation = Attenuation.None,
            };
    }
}