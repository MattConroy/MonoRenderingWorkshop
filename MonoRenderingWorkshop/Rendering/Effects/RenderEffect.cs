using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Rendering.Effects
{
    internal class RenderEffect : Effect, IEffectMatrices
    {
        public Vector3 LightAmbientColour { get; private set; }
        public Vector3 LightDiffuseColour { get; private set; }
        public Vector3 LightPosition { get; private set; }
        public Vector4 LightDirection { get; private set; }
        public Vector3 LightAttenuation { get; private set; }

        public RenderLight CurrentLight
        {
            set => AssignLightValues(value);
        }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public RenderEffect(Effect effect) : base(effect)
        {
            LightAmbientColour = Vector3.Zero;
            LightDiffuseColour = Vector3.Zero;
            LightPosition = Vector3.Zero;
            LightDirection = Vector4.Zero;
            LightAttenuation = Vector3.Zero;
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters[nameof(World)].SetValue(World);
            Parameters[nameof(View)].SetValue(View);
            Parameters[nameof(Projection)].SetValue(Projection);

            Parameters[nameof(LightAmbientColour)].SetValue(LightAmbientColour);
            Parameters[nameof(LightDiffuseColour)].SetValue(LightDiffuseColour);
            Parameters[nameof(LightPosition)].SetValue(LightPosition);
            Parameters[nameof(LightDirection)].SetValue(LightDirection);
            Parameters[nameof(LightAttenuation)].SetValue(LightAttenuation);
        }

        private void AssignLightValues(RenderLight light)
        {
            LightAmbientColour = light.AmbientColour.ToVector3();
            LightDiffuseColour = light.DiffuseColour.ToVector3();
            LightPosition = light.Position;
            LightDirection = light.GetDirection();
            LightAttenuation = light.GetAttenuation();
        }
    }
}