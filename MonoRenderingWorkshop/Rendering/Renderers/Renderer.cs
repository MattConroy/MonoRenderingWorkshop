using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRenderingWorkshop.Input;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;
using MonoRenderingWorkshop.Scenes.Cameras;
using System;
using System.Collections.Generic;

namespace MonoRenderingWorkshop.Rendering.Renderers
{
    internal abstract class Renderer : IDisposable
    {
        private static readonly Keys[] LightActivationKeys = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };
        private const int KeyOffset = (int)Keys.NumPad0;

        public bool AllLightsActive => ActiveLightIndex == -1;
        public int ActiveLightIndex { get; private set; }

        public abstract string MainEffectName { get; }
        public RenderEffect MainEffect { get; private set; }

        protected GraphicsDeviceManager DeviceManager { get; }

        private readonly KeyboardController _keyboard;

        protected Renderer(GraphicsDeviceManager graphics, int width, int height, KeyboardController keyboard)
        {
            DeviceManager = graphics;
            _keyboard = keyboard;

            Initialise(width, height);
            ActiveLightIndex = -1;
        }

        public virtual void SetMainEffect(Effect mainEffect)
        {
            MainEffect?.Dispose();
            MainEffect = CreateRenderEffect(mainEffect);
        }

        public virtual void Dispose()
        {
            MainEffect?.Dispose();
        }

        public void Update(GameTime time)
        {
            foreach (var key in LightActivationKeys)
                if (_keyboard.IsKeyDown(key))
                    ActiveLightIndex = (int)key - KeyOffset;

            if (_keyboard.IsKeyDown(Keys.Decimal))
                ActiveLightIndex = -1;
        }

        public void Draw(Camera camera, IList<RenderEntity> entities, IList<RenderLight> lights)
        {
            if (MainEffect == null)
                throw new InvalidOperationException($"{nameof(MainEffect)} cannot be null.");

            DeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            DeviceManager.GraphicsDevice.Clear(Color.Black);

            MainEffect.View = camera.View;
            MainEffect.Projection = camera.Projection;

            Draw(entities, lights);
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            if (_keyboard.IsKeyDown(Keys.X))
                DrawDebugInformation(spriteBatch);
        }

        protected abstract RenderEffect CreateRenderEffect(Effect mainEffect);

        protected abstract void Draw(IList<RenderEntity> entities, IList<RenderLight> lights);

        protected abstract void DrawDebugInformation(SpriteBatch spriteBatch);

        private void Initialise(int width, int height)
        {
            DeviceManager.SynchronizeWithVerticalRetrace = false;
            DeviceManager.PreferredBackBufferWidth = width;
            DeviceManager.PreferredBackBufferHeight = height;

            DeviceManager.ApplyChanges();
        }
    }
}
