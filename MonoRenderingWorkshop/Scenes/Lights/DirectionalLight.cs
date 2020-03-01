using Microsoft.Xna.Framework;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class DirectionalLight : Light
    {
        private readonly Vector3 _direction;

        public DirectionalLight(LightColour ambient, LightColour diffuse, Vector3 direction) : base(ambient, diffuse)
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
                AmbientColour = Ambient.ToColour(),
                DiffuseColour = Diffuse.ToColour(),
                Direction = _direction,
                DirectionFactor = 1,
                Position = Vector3.Zero,
                Attenuation = Attenuation.None,
            };
    }
}