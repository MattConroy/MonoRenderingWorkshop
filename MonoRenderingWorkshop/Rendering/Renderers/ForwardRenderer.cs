using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.MonoGame;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using System.Collections.Generic;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal sealed class ForwardRenderer : Renderer
    {
        public override string MainEffectName => "forwardRendering";

        private ForwardRenderingEffect _forwardRenderingEffect;
        private EffectTechnique _depthPrePass;
        private EffectTechnique _lightingPass;

        public ForwardRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            DeviceManager.PreferMultiSampling = true;
        }

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _forwardRenderingEffect = new ForwardRenderingEffect(mainEffect);

            _depthPrePass = _forwardRenderingEffect.Techniques.GetByName("DepthPrePass");
            _lightingPass = _forwardRenderingEffect.Techniques.GetByName("LightingPass");

            return _forwardRenderingEffect;
        }

        protected override void Draw(IList<RenderEntity> entities, IList<RenderLight> lights)
        {
            _forwardRenderingEffect.CurrentTechnique = _depthPrePass;

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

            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Additive;
            _forwardRenderingEffect.CurrentTechnique = _lightingPass;
            foreach (var entity in entities)
            {
                foreach (var mesh in entity.Model.Meshes)
                {
                    _forwardRenderingEffect.World = entity.BoneTransforms[mesh.ParentBone.Index] * entity.World;

                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = _forwardRenderingEffect;
                    }

                    for (var i = 0; i < lights.Count; ++i)
                    {
                        if (AllLightsActive || ActiveLightIndex == i)
                        {
                            _forwardRenderingEffect.CurrentLight = lights[i];
                            mesh.Draw();
                        }
                    }
                }
            }
        }

        protected override void DrawDebugInformation(SpriteBatch spriteBatch) { }
    }
}