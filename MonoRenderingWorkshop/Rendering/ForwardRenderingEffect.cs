using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering
{
    internal sealed class ForwardRenderingEffect : RenderEffect
    {
        private const int MaxLights = 8;

        public RenderLight[] Lights
        {
            set => AssignLightValues(value);
        }

        private readonly Vector3[] _directions;
        private readonly Vector3[] _colours;

        public ForwardRenderingEffect(Effect effect) : base(effect)
        {
            _directions = new Vector3[MaxLights];
            _colours = new Vector3[MaxLights];
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters[$"Light{nameof(RenderLight.Direction)}"].SetValue(_directions);
            Parameters[$"Light{nameof(RenderLight.Colour)}"].SetValue(_colours);
        }

        private void AssignLightValues(RenderLight[] lights)
        {
            for (int i = 0; i < MaxLights; ++i)
            {
                if (i < lights.Length)
                {
                    _directions[i] = lights[i].Direction;
                    _colours[i] = lights[i].Colour.ToVector3();
                }
                else
                {
                    _directions[i] = Vector3.Zero;
                    _colours[i] = Vector3.Zero;
                }
            }
        }
    }
}