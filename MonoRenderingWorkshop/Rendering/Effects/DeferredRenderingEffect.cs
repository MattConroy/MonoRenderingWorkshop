using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Effects
{
    internal sealed class DeferredRenderingEffect : RenderEffect
    {
        public Texture PositionBuffer { get; set; }
        public Texture NormalBuffer { get; set; }

        public DeferredRenderingEffect(Effect effect) : base(effect)
        {
        }

        protected override void OnApply()
        {
            base.OnApply();

            Parameters["InverseViewProjection"].SetValue(Matrix.Invert(View * Projection));

            Parameters[nameof(PositionBuffer)]?.SetValue(PositionBuffer);
            Parameters[nameof(NormalBuffer)]?.SetValue(NormalBuffer);
        }
    }
}