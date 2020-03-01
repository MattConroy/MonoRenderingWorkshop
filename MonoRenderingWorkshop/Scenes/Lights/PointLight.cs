using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class PointLight : Light
    {
        private readonly Vector3 _position;
        private readonly float _size;

        public PointLight(LightColour ambient, LightColour diffuse, Vector3 position, float size) : base(ambient, diffuse)
        {
            Position = position;
            _position = position;
            _size = size;
        }

        public override void Update(GameTime time)
        {
        }

        public override RenderLight GetRenderData() =>
            new RenderLight
            {
                AmbientColour = Ambient.ToColour(),
                DiffuseColour = Diffuse.ToColour(),
                Direction = Vector3.Zero,
                DirectionFactor = 0,
                Position = Position,
                Attenuation = Attenuation.Range(_size)
            };
    }
}