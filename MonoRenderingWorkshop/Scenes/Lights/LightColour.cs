using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class LightColour
    {
        private readonly float _intensity;
        private readonly Color _colour;

        public LightColour(Color colour, float intensity)
        {
            _intensity = intensity;
            _colour = colour;
        }

        public Color ToColour() => _colour * _intensity;
    }
}