using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering
{
    internal class RenderEffect : Effect, IEffectMatrices
    {
        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public RenderEffect(Effect effect) : base(effect)
        {
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters[nameof(World)].SetValue(World);
            Parameters[nameof(View)].SetValue(View);
            Parameters[nameof(Projection)].SetValue(Projection);
        }
    }
}