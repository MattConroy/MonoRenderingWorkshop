namespace MonoRenderingWorkshop.Rendering.Effects.Parameters
{
    internal struct Attenuation
    {
        public float ConstantFactor { get; set; }
        public float LinearFactor { get; set; }
        public float ExponentialFactor { get; set; }

        public static Attenuation None => Constant(1);

        public static Attenuation Constant(float factor) =>
            new Attenuation
            {
                ConstantFactor = factor,
                LinearFactor = 0,
                ExponentialFactor = 0,
            };

        public static Attenuation Linear(float factor) =>
            new Attenuation
            {
                ConstantFactor = 1,
                LinearFactor = factor,
                ExponentialFactor = 0,
            };

        public static Attenuation Exponential(float factor) =>
            new Attenuation
            {
                ConstantFactor = 1,
                LinearFactor = 0,
                ExponentialFactor = factor,
            };

        public static Attenuation Range(float size) =>
            new Attenuation
            {
                ConstantFactor = 0.5f,
                LinearFactor = 1 / size,
                ExponentialFactor = 1 / size,
            };
    }
}