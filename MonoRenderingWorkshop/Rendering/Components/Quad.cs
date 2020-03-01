using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Components
{
    internal class Quad 
    {
        private readonly VertexDeclaration _vertexDeclaration;
        private readonly VertexPositionTexture[] _vertices;
        private readonly short[] _indices;
        private readonly int _primitiveCount;

        private readonly GraphicsDevice _graphicsDevice;

        public Quad(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

            _vertexDeclaration = VertexPositionTexture.VertexDeclaration;
            _vertices = new[]
            {
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,0)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,0))
            };
            _indices = new short[] { 0, 1, 2, 2, 3, 0 };
            _primitiveCount = _indices.Length / 3;
        }

        public void Draw(EffectTechnique technique, Vector2 corner1, Vector2 corner2)
        {
            SetVertices(corner1, corner2);
            Draw(technique);
        }

        public void Draw(EffectTechnique technique)
        {
            foreach (var pass in technique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length,
                    _indices, 0, _primitiveCount,
                    _vertexDeclaration);
            }
        }

        protected void SetVertices(Vector2 corner1, Vector2 corner2)
        {
            _vertices[0].Position.X = corner2.X;
            _vertices[0].Position.Y = corner1.Y;

            _vertices[1].Position.X = corner1.X;
            _vertices[1].Position.Y = corner1.Y;

            _vertices[2].Position.X = corner1.X;
            _vertices[2].Position.Y = corner2.Y;

            _vertices[3].Position.X = corner2.X;
            _vertices[3].Position.Y = corner2.Y;
        }
    }

}
