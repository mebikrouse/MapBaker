using System.Collections.Generic;
using System.Linq;
using MapBaker.Utility.Converting;
using UnityEngine;

namespace MapBaker.Masking
{
    public class WaterMask : IMask
    {
        private const float RAYCAST_DISTANCE = 100f;

        private readonly IConverter<Vector2Int, Vector3> _coordinateConverter;

        private readonly float _minOpacity;
        private readonly float _maxDepth;

        private readonly int _raycastMask;

        public WaterMask(IConverter<Vector2Int, Vector3> coordinateConverter, float minOpacity, float maxDepth,
            IEnumerable<string> raycastLayers)
        {
            _coordinateConverter = coordinateConverter;

            _minOpacity = minOpacity;
            _maxDepth = maxDepth;

            _raycastMask = LayerMask.GetMask(raycastLayers.ToArray());
        }

        public float GetOpacity(int x, int y)
        {
            var position = _coordinateConverter.Convert(new Vector2Int(x, y));

            var waterLevel = TerrainMeta.WaterMap.GetHeight(position);
            var worldHeight = TerrainMeta.HeightMap.GetHeight(position);

            if (Physics.Raycast(position, Vector3.down, out var hit, RAYCAST_DISTANCE, _raycastMask,
                QueryTriggerInteraction.Ignore))
                worldHeight = hit.point.y;

            return waterLevel > worldHeight ? Mathf.Clamp01(_minOpacity + (waterLevel - worldHeight) / _maxDepth) : 0;
        }
    }
}