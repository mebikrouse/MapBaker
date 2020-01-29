using MapBaker.Utility.Converting;
using UnityEngine;

namespace MapBaker.Masking
{
    public class SplatMask : IMask
    {
        private readonly IConverter<Vector2Int, Vector3> _coordinateConverter;
        private readonly int _splatType;

        public SplatMask(IConverter<Vector2Int, Vector3> coordinateConverter, int splatIndex)
        {
            _coordinateConverter = coordinateConverter;
            _splatType = TerrainSplat.IndexToType(splatIndex);
        }

        public float GetOpacity(int x, int y)
        {
            var position = _coordinateConverter.Convert(new Vector2Int(x, y));
            return TerrainMeta.SplatMap.GetSplat(position, _splatType);
        }
    }
}