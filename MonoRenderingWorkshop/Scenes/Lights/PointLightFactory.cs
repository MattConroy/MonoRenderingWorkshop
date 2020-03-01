using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRenderingWorkshop.Scenes.Lights
{
    internal sealed class PointLightFactory
    {
        private readonly IEnumerator<Color> _colourEnumerator;

        public PointLightFactory()
        {
            _colourEnumerator = LightColours().GetEnumerator();
        }

        public Light CreatePointLight(LightColour ambientLight, float diffuseIntensity, float size)
        {
            _colourEnumerator.MoveNext();
            return new PointLight(ambientLight, new LightColour(_colourEnumerator.Current, diffuseIntensity), Vector3.Zero, size);
        }

        private static IEnumerable<Color> LightColours()
        {
            while (true)
            {
                yield return Color.Red;
                yield return Color.Orange;
                yield return Color.Yellow;
                yield return Color.Green;
                yield return Color.Blue;
                yield return Color.Indigo;
                yield return Color.Violet;

                yield return Color.Violet;
                yield return Color.Indigo;
                yield return Color.Blue;
                yield return Color.Green;
                yield return Color.Yellow;
                yield return Color.Orange;
                yield return Color.Red;
            }
        }
    }
}