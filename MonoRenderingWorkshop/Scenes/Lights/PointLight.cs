using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class PointLight : Light
    {
        private readonly Vector3 _position;
        private readonly float _size;

        public PointLight(Vector3 position, Color colour, float intensity, float size) : base(colour, intensity)
        {
            _position = position;
            _size = size;
        }

        public override void Update(GameTime time)
        {
        }

        public override RenderLight GetRenderData() =>
            new RenderLight
            {
                Colour = Colour,
                Intensity = Intensity,
                Direction = Vector3.Zero,
                DirectionFactor = 0,
                Position = new Vector3(_position.X, _position.Y, _position.Z),
                Attenuation = Attenuation.Range(_size)
            };
    }
}