using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.MonoGame;
using MonoRenderingWorkshop.Rendering.Components;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using MonoRenderingWorkshop.Scenes.Cameras;
using System;
using System.Collections.Generic;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal sealed class DeferredRenderer : Renderer
    {
        private readonly FullScreenQuad _fullScreenQuad;

        private readonly RenderTarget2D _positionBuffer;
        private readonly RenderTarget2D _normalBuffer;

        private readonly Texture2D _positionTexture;
        private readonly Texture2D _normalTexture;

        private readonly Color[] _positionData;
        private readonly Color[] _normalData;

        private DeferredRenderingEffect _deferredRenderingEffect;

        private EffectTechnique _clearBufferTechnique;
        private EffectTechnique _renderGeometryTechnique;
        private EffectTechnique _renderLightingTechnique;

        public DeferredRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            _positionBuffer = new RenderTarget2D(DeviceManager.GraphicsDevice, width, height, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            _normalBuffer = new RenderTarget2D(DeviceManager.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            _positionTexture = new Texture2D(DeviceManager.GraphicsDevice, width, height, false, _positionBuffer.Format);
            _normalTexture = new Texture2D(DeviceManager.GraphicsDevice, width, height, false, _normalBuffer.Format);

            _positionData = new Color[width * height];
            _normalData = new Color[width * height];

            _fullScreenQuad = new FullScreenQuad(DeviceManager.GraphicsDevice);

            DeviceManager.PreferMultiSampling = false;
        }

        public override void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights)
        {
            if (_deferredRenderingEffect == null)
                throw new InvalidOperationException($"{nameof(_deferredRenderingEffect)} cannot be null.");

            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            DeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            DeviceManager.GraphicsDevice.SetRenderTargets(_positionBuffer, _normalBuffer);

            _deferredRenderingEffect.View = camera.View;
            _deferredRenderingEffect.Projection = camera.Projection;

            _fullScreenQuad.Draw(_clearBufferTechnique);

            //_deferredRenderingEffect.Lights = lights.ToArray();
            _deferredRenderingEffect.CurrentTechnique = _renderGeometryTechnique;

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



            DeviceManager.GraphicsDevice.SetRenderTarget(null);

            foreach (var light in lights)
            {
                _deferredRenderingEffect.CurrentLight = light;
                _fullScreenQuad.Draw(_renderLightingTechnique);
            }

        }

        public override void DrawDebug(SpriteBatch spriteBatch)
        {
            int halfWidth = DeviceManager.GraphicsDevice.Viewport.Width / 2;
            int halfHeight = DeviceManager.GraphicsDevice.Viewport.Height / 2;

            _positionBuffer.GetData(_positionData);
            _positionTexture.SetData(_positionData);

            _normalBuffer.GetData(_normalData);
            _normalTexture.SetData(_normalData);

            spriteBatch.Draw(_positionTexture, new Rectangle(0, halfHeight, halfWidth, halfHeight), Color.White);
            spriteBatch.Draw(_normalTexture, new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);
        }

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _deferredRenderingEffect = new DeferredRenderingEffect(mainEffect);
            _clearBufferTechnique = _deferredRenderingEffect.Techniques.GetByName("ClearBuffer");
            _renderGeometryTechnique = _deferredRenderingEffect.Techniques.GetByName("RenderGeometry");
            _renderLightingTechnique = _deferredRenderingEffect.Techniques.GetByName("RenderLighting");
            return _deferredRenderingEffect;
        }
    }
}