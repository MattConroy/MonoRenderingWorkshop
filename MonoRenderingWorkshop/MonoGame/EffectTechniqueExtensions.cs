using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoRenderingWorkshop.MonoGame
{
    public static class EffectTechniqueExtensions
    {
        public static EffectTechnique GetByName(this IEnumerable<EffectTechnique> techniques, string name) =>
            techniques.First(technique => technique.Name == name);

        public static EffectTechnique FindByName(this IEnumerable<EffectTechnique> techniques, string name) =>
            techniques.FirstOrDefault(technique => technique.Name == name);
    }
}
