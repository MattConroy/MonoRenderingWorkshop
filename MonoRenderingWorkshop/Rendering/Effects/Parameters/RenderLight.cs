using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Rendering.Effects.Parameters
{
    internal struct RenderLight
    {
        public Color Colour { get; set; }
        public float Intensity { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public float DirectionFactor { get; set; }
        public Attenuation Attenuation { get; set; }
    }
}