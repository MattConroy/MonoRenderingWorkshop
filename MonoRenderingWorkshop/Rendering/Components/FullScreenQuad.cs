using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Components
{
    internal sealed class FullScreenQuad : Quad
    {
        public FullScreenQuad(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            SetVertices(-Vector2.One, Vector2.One);
        }
    }
}
