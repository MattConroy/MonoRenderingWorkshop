using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Scenes.Cameras;
using MonoRenderingWorkshop.Scenes.Lights;
using System.Collections.Generic;

namespace MonoRenderingWorkshop.Rendering
{
    internal abstract class Renderer
    {
        public RenderEffect MainEffect { get; private set; }

        protected GraphicsDeviceManager DeviceManager { get; }

        protected Renderer(GraphicsDeviceManager graphics, int width, int height)
        {
            DeviceManager = graphics;
            Initialise(width, height);
        }

        private void Initialise(int width, int height)
        {
            DeviceManager.SynchronizeWithVerticalRetrace = false;
            DeviceManager.PreferredBackBufferWidth = width;
            DeviceManager.PreferredBackBufferHeight = height;

            DeviceManager.ApplyChanges();
        }

        public virtual void SetMainEffect(Effect mainEffect)
        {
            MainEffect = CreateRenderEffect(mainEffect);
        }

        public abstract void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights);

        protected abstract RenderEffect CreateRenderEffect(Effect mainEffect);
    }
}
