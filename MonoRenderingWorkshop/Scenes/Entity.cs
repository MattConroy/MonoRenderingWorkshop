using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoRenderingWorkshop.Rendering;
using MonoRenderingWorkshop.Rendering.Effects;
using MonoRenderingWorkshop.Rendering.Effects.Parameters;

namespace MonoRenderingWorkshop.Scenes
{
    internal sealed class Entity
    {
        private readonly RenderEntity _renderData;

        private readonly Quaternion _rotation;
        private readonly Vector3 _position;
        private readonly Vector3 _scale;

        public Entity(Model model, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _renderData = new RenderEntity(model);
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public RenderEntity GetRenderData()
        {
            var rotation = Matrix.CreateFromQuaternion(_rotation);
            _renderData.World = Matrix.CreateScale(_scale) *
                                Matrix.CreateWorld(_position,
                                    Vector3.TransformNormal(Vector3.Forward, rotation),
                                    Vector3.TransformNormal(Vector3.Up, rotation));
            return _renderData;
        }
    }
}