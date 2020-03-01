using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using MonoRenderingWorkshop.Scenes.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal sealed class DeferredRenderer : Renderer
    {
        private DeferredRenderingEffect _deferredRenderingEffect;

        private RenderTarget2D _positionBuffer;

        public DeferredRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            _positionBuffer = new RenderTarget2D(graphics.GraphicsDevice, width, height, false, SurfaceFormat.Rg32, `);
        }

        public override void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights)
        {
            if (MainEffect == null)
                throw new InvalidOperationException($"{nameof(MainEffect)} cannot be null.");

            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            DeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _deferredRenderingEffect.View = camera.View;
            _deferredRenderingEffect.Projection = camera.Projection;
            _deferredRenderingEffect.Lights = lights.ToArray();

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

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _deferredRenderingEffect = new DeferredRenderingEffect(mainEffect);
            return _deferredRenderingEffect;
        }
    }
}