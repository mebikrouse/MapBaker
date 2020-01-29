using UnityEngine;

namespace MapBaker.Utility.Converting
{
    public class RaycastCoordinateConverter : IConverter<Vector2Int, Vector3>
    {
        private const float HEIGHT_MARGIN = 75f;

        private readonly IConverter<Vector2Int, Vector3> _coordinateConverter;

        public RaycastCoordinateConverter(IConverter<Vector2Int, Vector3> coordinateConverter)
        {
            _coordinateConverter = coordinateConverter;
        }

        public Vector3 Convert(Vector2Int input)
        {
            var position = _coordinateConverter.Convert(input);
            return new Vector3(position.x, TerrainMeta.HeightMap.GetHeight(position) + HEIGHT_MARGIN, position.z);
        }
    }
}