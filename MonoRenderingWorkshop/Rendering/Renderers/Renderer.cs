using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Scenes.Cameras;
using System.Collections.Generic;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal abstract class Renderer
    {
        private static readonly Keys[] LightActivationKeys = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };
        private const int KeyOffset = (int) Keys.NumPad0;

        public RenderEffect MainEffect { get; private set; }

        protected GraphicsDeviceManager DeviceManager { get; }

        private readonly KeyboardController _keyboard;

        protected Renderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard)
        {
            DeviceManager = graphics;
            _keyboard = keyboard;

            Initialise(width, height);
        }

        public void Update(GameTime time)
        {
            foreach (var key in LightActivationKeys)
                if (_keyboard.IsKeyDown(key))
                    MainEffect.SetActiveLight((int)key - KeyOffset);

            if (_keyboard.IsKeyDown(Keys.Decimal))
                MainEffect.SetActiveLight(-1);
        }

        public virtual void SetMainEffect(Effect mainEffect)
        {
            MainEffect = CreateRenderEffect(mainEffect);
        }

        public abstract void Draw(Camera camera, IEnumerable<RenderEntity> entities, IEnumerable<RenderLight> lights);

        protected abstract RenderEffect CreateRenderEffect(Effect mainEffect);

        private void Initialise(int width, int height)
        {
            DeviceManager.SynchronizeWithVerticalRetrace = false;
            DeviceManager.PreferredBackBufferWidth = width;
            DeviceManager.PreferredBackBufferHeight = height;

            DeviceManager.ApplyChanges();
        }
    }
}
