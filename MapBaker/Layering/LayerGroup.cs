using System.Collections.Generic;
using MapBaker.Bitmapping;

namespace MapBaker.Layering
{
    public class LayerGroup : ILayer
    {
        private readonly IEnumerable<ILayer> _layers;

        public LayerGroup(IEnumerable<ILayer> layers)
        {
            _layers = layers;
        }

        public void Draw(IBitmap canvas)
        {
            foreach (var layer in _layers)
                layer.Draw(canvas);
        }
    }
}