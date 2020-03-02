using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.MonoGame;
using MonoRenderingWorkshop.Rendering.Components;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using System.Collections.Generic;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal sealed class DeferredRenderer : Renderer
    {
        public override string MainEffectName => "deferredRendering";

        private readonly FullScreenQuad _fullScreenQuad;

        private readonly RenderTexture _positionBuffer;
        private readonly RenderTexture _normalBuffer;

        private DeferredRenderingEffect _deferredRenderingEffect;

        private EffectTechnique _clearBufferPass;
        private EffectTechnique _geometryPass;
        private EffectTechnique _lightingPass;

        public DeferredRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            _positionBuffer = new RenderTexture(DeviceManager.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            _normalBuffer = new RenderTexture(DeviceManager.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);

            _fullScreenQuad = new FullScreenQuad(DeviceManager.GraphicsDevice);

            DeviceManager.PreferMultiSampling = false;
        }

        public override void Dispose()
        {
            base.Dispose();

            _positionBuffer?.Dispose();
            _normalBuffer?.Dispose();
        }

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _deferredRenderingEffect = new DeferredRenderingEffect(mainEffect);

            _clearBufferPass = _deferredRenderingEffect.Techniques.GetByName("ClearBufferPass");
            _geometryPass = _deferredRenderingEffect.Techniques.GetByName("GeometryPass");
            _lightingPass = _deferredRenderingEffect.Techniques.GetByName("LightingPass");

            _deferredRenderingEffect.PositionBuffer = _positionBuffer;
            _deferredRenderingEffect.NormalBuffer = _normalBuffer;

            return _deferredRenderingEffect;
        }

        protected override void Draw(IList<RenderEntity> entities, IList<RenderLight> lights)
        {
            DeviceManager.GraphicsDevice.SetRenderTargets(_positionBuffer, _normalBuffer);

            ClearGeometryBuffers();

            DrawGeometryPass(entities);

            DeviceManager.GraphicsDevice.SetRenderTarget(null);
            DeviceManager.GraphicsDevice.BlendState = BlendState.Additive;

            DrawLightingPass(lights);
        }

        protected override void DrawDebugInformation(SpriteBatch spriteBatch)
        {
            int halfWidth = DeviceManager.GraphicsDevice.Viewport.Width / 2;
            int halfHeight = DeviceManager.GraphicsDevice.Viewport.Height / 2;
            spriteBatch.Draw(_positionBuffer.ToTexture2D(), new Rectangle(0, halfHeight, halfWidth, halfHeight), Color.White);
            spriteBatch.Draw(_normalBuffer.ToTexture2D(), new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);
        }

        private void ClearGeometryBuffers()
        {
            _fullScreenQuad.Draw(_clearBufferPass);
        }

        private void DrawGeometryPass(IEnumerable<RenderEntity> entities)
        {
            _deferredRenderingEffect.CurrentTechnique = _geometryPass;

            foreach (var entity in entities)
            {
                foreach (var mesh in entity.Model.Meshes)
                {
                    _deferredRenderingEffect.World = entity.BoneTransforms[mesh.ParentBone.Index] * entity.World;

                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = _deferredRenderingEffect;
                    }

                    mesh.Draw();
                }
            }
        }

        private void DrawLightingPass(IList<RenderLight> lights)
        {
            for (var i = 0; i < lights.Count; ++i)
            {
                if (AllLightsActive || ActiveLightIndex == i)
                {
                    _deferredRenderingEffect.CurrentLight = lights[i];
                    _fullScreenQuad.Draw(_lightingPass);
                }
            }
        }
    }
}