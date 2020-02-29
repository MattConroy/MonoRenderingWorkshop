using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Scenes.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoRenderingWorkshop.Rendering
{
    internal sealed class ForwardRenderer : Renderer
    {
        private ForwardRenderingEffect _forwardRenderingEffect;

        public ForwardRenderer(GraphicsDeviceManager graphics, int width, int height) : base(graphics, width, height)
        {
        }

        public override void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights)
        {
            if (MainEffect == null)
                throw new InvalidOperationException($"{nameof(MainEffect)} cannot be null.");

            DeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;

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

        protected override RenderEffect CreateRenderEffect(Effect mainEffect)
        {
            _forwardRenderingEffect = new ForwardRenderingEffect(mainEffect);
            return _forwardRenderingEffect;
        }
    }
}