using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Rendering.Effects.Parameters
{
    internal struct RenderLight
    {
        public Color DiffuseColour { get; set; }
        public Color AmbientColour { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public float DirectionFactor { get; set; }
        public Attenuation Attenuation { get; set; }

        public Vector4 GetDirection() => new Vector4(Direction, DirectionFactor);
        public Vector3 GetAttenuation() => 
            new Vector3(
                Attenuation.ConstantFactor,
                Attenuation.LinearFactor,
                Attenuation.ExponentialFactor);
    }
}