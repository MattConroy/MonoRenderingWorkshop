using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRenderingWorkshop.Rendering.Effects.Parameters
{
    internal class RenderEntity
    {
        public Matrix World { get; set; }
        public Matrix[] BoneTransforms { get; }
        public Model Model { get; }

        public RenderEntity(Model model)
        {
            Model = model;
            BoneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(BoneTransforms);
        }
    }
}