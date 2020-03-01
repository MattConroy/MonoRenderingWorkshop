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
    internal sealed class ForwardRenderer : Renderer
    {
        public override string MainEffectName => "forwardRendering";

        private ForwardRenderingEffect _forwardRenderingEffect;

        public ForwardRenderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard) : base(graphics, width, height, keyboard)
        {
            DeviceManager.PreferMultiSampling = true;
        }

        public override void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights)
        {
            if (_forwardRenderingEffect == null)
                throw new InvalidOperationException($"{nameof(_forwardRenderingEffect)} cannot be null.");

            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            DeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            _forwardRenderingEffect.View = camera.View;
            _forwardRenderingEffect.Projection = camera.Projection;
            _forwardRenderingEffect.Lights = lights.ToArray();

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

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _forwardRenderingEffect = new ForwardRenderingEffect(mainEffect);
            return _forwardRenderingEffect;
        }
    }
}