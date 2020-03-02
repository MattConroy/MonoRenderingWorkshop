using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal sealed class ForwardRenderer : Renderer
    {
        public override string MainEffectName => "forwardRendering";

        private ForwardRenderingEffect _forwardRenderingEffect;

        public ForwardRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            DeviceManager.PreferMultiSampling = true;
        }

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _forwardRenderingEffect = new ForwardRenderingEffect(mainEffect);
            return _forwardRenderingEffect;
        }

        protected override void Draw(IList<RenderEntity> entities, IList<RenderLight> lights)
        {
            _forwardRenderingEffect.CurrentLight = lights.First();

            foreach (var entity in entities)
            {
                foreach (var mesh in entity.Model.Meshes)
                {
                    _forwardRenderingEffect.World = entity.BoneTransforms[mesh.ParentBone.Index] * entity.World;

                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = _forwardRenderingEffect;
                    }

                    mesh.Draw();
                }
            }
        }

        protected override void DrawDebugInformation(SpriteBatch spriteBatch) { }
    }
}