using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Rendering.Effects
{
    internal sealed class DeferredRenderingEffect : RenderEffect
    {
        public Texture PositionBuffer { get; set; }
        public Texture NormalBuffer { get; set; }
        public RenderLight CurrentLight
        {
            set => AssignLightValue(value);
        }

        private Vector3 _attenuation;
        private Vector4 _direction;
        private Vector3 _position;
        private Vector3 _colour;

        public DeferredRenderingEffect(Effect effect) : base(effect)
        {
            _attenuation = Vector3.Zero;
            _direction = Vector4.Zero;
            _position = Vector3.Zero;
            _colour = Vector3.Zero;
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters["InverseViewProjection"].SetValue(Matrix.Invert(View * Projection));

            Parameters[$"Light{nameof(RenderLight.Attenuation)}"].SetValue(_attenuation);
            Parameters[$"Light{nameof(RenderLight.Direction)}"].SetValue(_direction);
            Parameters[$"Light{nameof(RenderLight.Position)}"].SetValue(_position);
            Parameters[$"Light{nameof(RenderLight.Colour)}"].SetValue(_colour);

            Parameters[nameof(PositionBuffer)]?.SetValue(PositionBuffer);
            Parameters[nameof(NormalBuffer)]?.SetValue(NormalBuffer);
        }

        private void AssignLightValue(RenderLight light)
        {
            _attenuation = light.GetAttenuation();
            _direction = light.GetDirection();
            _position = light.Position;
            _colour = light.GetColour();
        }
    }
}