using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Rendering.Effects
{
    internal sealed class ForwardRenderingEffect : RenderEffect
    {
        private const int MaxLights = 4;

        public RenderLight[] Lights
        {
            set => AssignLightValues(value);
        }

        private readonly Vector3[] _attenuation;
        private readonly Vector4[] _directions;
        private readonly Vector3[] _positions;
        private readonly Vector3[] _colours;

        public ForwardRenderingEffect(Effect effect) : base(effect)
        {
            _attenuation = new Vector3[MaxLights];
            _directions = new Vector4[MaxLights];
            _positions = new Vector3[MaxLights];
            _colours = new Vector3[MaxLights];
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters[$"Light{nameof(RenderLight.Attenuation)}"].SetValue(_attenuation);
            Parameters[$"Light{nameof(RenderLight.Direction)}"].SetValue(_directions);
            Parameters[$"Light{nameof(RenderLight.Position)}"].SetValue(_positions);
            Parameters[$"Light{nameof(RenderLight.Colour)}"].SetValue(_colours);
        }

        private void AssignLightValues(RenderLight[] lights)
        {
            for (int i = 0; i < MaxLights; ++i)
            {
                if (i < lights.Length && (AllLightsActive || ActiveLightIndex == i))
                {
                    _attenuation[i] = new Vector3(
                        lights[i].Attenuation.ConstantFactor,
                        lights[i].Attenuation.LinearFactor,
                        lights[i].Attenuation.ExponentialFactor);

                    _directions[i] = new Vector4(
                        lights[i].Direction,
                        lights[i].DirectionFactor);

                    _positions[i] = lights[i].Position;

                    _colours[i] = lights[i].Intensity *
                                  lights[i].Colour.ToVector3();
                }
                else
                {
                    _attenuation[i] = Vector3.Zero;
                    _directions[i] = Vector4.Zero;
                    _positions[i] = Vector3.Zero;
                    _colours[i] = Vector3.Zero;
                }
            }
        }
    }
}