using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Effects
{
    internal class RenderEffect : Effect, IEffectMatrices
    {
        public bool AllLightsActive => ActiveLightIndex == -1;
        public int ActiveLightIndex { get; private set; }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }


        public RenderEffect(Effect effect) : base(effect)
        {
            SetActiveLight(-1);
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters[nameof(World)].SetValue(World);
            Parameters[nameof(View)].SetValue(View);
            Parameters[nameof(Projection)].SetValue(Projection);
        }

        public void SetActiveLight(int index)
        {
            ActiveLightIndex = index;
        }
    }
}