using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Rendering
{
    internal struct RenderLight
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public float Intensity { get; set; }
        public float ConeAngle { get; set; }
        public Color Colour { get; set; }
    }
}