using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Components
{
    internal class RenderTexture : RenderTarget2D
    {
        private readonly Texture2D _texture;
        private readonly Color[] _data;

        public RenderTexture(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat) : base(graphicsDevice, width, height, mipMap, preferredFormat, preferredDepthFormat)
        {
            _texture = new Texture2D(graphicsDevice, Width, Height, mipMap, Format);
            _data = new Color[width * height];
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _texture?.Dispose();
        }

        public Texture2D ToTexture2D()
        {
            GetData(_data);
            _texture.SetData(_data);
            return _texture;
        }
    }
}