using UnityEngine;

namespace MapBaker.Utility.Converting
{
    public class CoordinateConverter : IConverter<Vector2Int, Vector3>
    {
        private readonly Vector2 _ratio;
        private readonly Vector3 _terrainPosition;

        public CoordinateConverter(int width, int height)
        {
            _ratio = new Vector2(TerrainMeta.Size.x / width, TerrainMeta.Size.z / height);
            _terrainPosition = TerrainMeta.Position;
        }

        public Vector3 Convert(Vector2Int input)
        {
            return new Vector3(_ratio.x * input.x + _terrainPosition.x, 0, _ratio.y * input.y + _terrainPosition.z);
        }
    }
}