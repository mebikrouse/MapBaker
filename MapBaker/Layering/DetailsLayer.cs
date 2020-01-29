using System;
using System.Collections.Generic;
using System.Linq;
using MapBaker.Bitmapping;
using MapBaker.Blending;
using MapBaker.Configuration;
using MapBaker.Utility.Converting;
using MapBaker.Utility.Providers;
using UnityEngine;

namespace MapBaker.Layering
{
    public class DetailsLayer : ILayer
    {
        public class MaterialSource : IDisposable
        {
            private struct Material : IDisposable
            {
                private readonly IEnumerable<string> _patterns;

                public IBitmap Bitmap { get; }

                public Material(IBitmap bitmap, IEnumerable<string> patterns)
                {
                    Bitmap = bitmap;
                    _patterns = patterns;
                }

                public bool CheckForPatterns(string gameObjectName)
                {
                    foreach (var pattern in _patterns)
                        if (gameObjectName.Contains(pattern))
                            return true;

                    return false;
                }

                public void Dispose()
                {
                    (Bitmap as IDisposable)?.Dispose();
                }
            }

            private readonly List<Material> _materials;

            public MaterialSource(IEnumerable<MaterialConfiguration> materialConfigurations, int width, int height)
            {
                _materials = new List<Material>();
                foreach (var configuration in materialConfigurations)
                {
                    var bitmapProvider =
                        new SeamlessProvider(new BitmapProvider(configuration.TexturePath.FullPath), width, height);
                    _materials.Add(new Material(bitmapProvider.GetItem(), configuration.Patterns));
                }
            }

            public IBitmap GetTexture(string gameObjectName)
            {
                foreach (var material in _materials)
                    if (material.CheckForPatterns(gameObjectName))
                        return material.Bitmap;

                return null;
            }

            public void Dispose()
            {
                foreach (var material in _materials) material.Dispose();
            }
        }

        private const float RAYCAST_DISTANCE = 100f;

        private readonly IConverter<Vector2Int, Vector3> _coordinateConverter;
        private readonly MaterialSource _materialSource;
        private readonly int _raycastMask;

        public DetailsLayer(IConverter<Vector2Int, Vector3> coordinateConverter, MaterialSource materialSource,
            IEnumerable<string> raycastLayers)
        {
            _coordinateConverter = coordinateConverter;
            _materialSource = materialSource;
            _raycastMask = LayerMask.GetMask(raycastLayers.ToArray());
        }

        public void Draw(IBitmap canvas)
        {
            for (var x = 0; x < canvas.Width; x++)
            {
                for (var y = 0; y < canvas.Height; y++)
                {
                    var position = _coordinateConverter.Convert(new Vector2Int(x, y));

                    if (!Physics.Raycast(position, Vector3.down, out var hit, RAYCAST_DISTANCE, _raycastMask,
                        QueryTriggerInteraction.Ignore)) continue;

                    var materialBitmap = _materialSource.GetTexture(hit.collider.gameObject.name.ToLower());
                    if (materialBitmap == null) continue;

                    canvas.SetPixel(x, y,
                        NormalColorBlending.Instance.Blend(canvas.GetPixel(x, y), materialBitmap.GetPixel(x, y)));
                }
            }
        }
    }
}