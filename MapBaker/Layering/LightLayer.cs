using System.Collections.Generic;
using System.Linq;
using MapBaker.Bitmapping;
using MapBaker.Configuration;
using MapBaker.Utility.Converting;
using UnityEngine;

namespace MapBaker.Layering
{
    public class LightLayer : ILayer
    {
        private const float RAYCAST_DISTANCE = 100f;
        private const float PRECISION = 0.1f;
        private const float PRECISION_COS = PRECISION * 0.866f;
        private const float PRECISION_SIN = PRECISION * 0.5f;

        private readonly IConverter<Vector2Int, Vector3> _coordinateConverter;
        private readonly LightConfiguration _configuration;
        private readonly int _raycastMask;

        public LightLayer(IConverter<Vector2Int, Vector3> coordinateConverter, LightConfiguration configuration,
            IEnumerable<string> raycastLayers)
        {
            _coordinateConverter = coordinateConverter;
            _configuration = configuration;
            _raycastMask = LayerMask.GetMask(raycastLayers.ToArray());
        }

        public void Draw(IBitmap canvas)
        {
            var lightDirection = _configuration.LightDirection.normalized;
            var reversedLightDirection = -lightDirection;
            var reversedViewDirection = -Vector3.down;

            for (var x = 0; x < canvas.Width; x++)
            {
                for (var y = 0; y < canvas.Height; y++)
                {
                    var position = _coordinateConverter.Convert(new Vector2Int(x, y));

                    var normal = GetNormal(position);

                    var diffuseComponent = GetDiffuse(normal, reversedLightDirection);
                    var specularComponent = GetSpecular(normal, lightDirection, reversedViewDirection);

                    var canvasColor = canvas.GetPixel(x, y);
                    var resultingColor = GetLightComponent(canvasColor, _configuration.AmbientColor,
                                             _configuration.AmbientIntensity) +
                                         GetLightComponent(canvasColor, _configuration.LightColor,
                                             _configuration.LightIntensity * diffuseComponent) +
                                         GetSpecularComponent(_configuration.SpecularColor, _configuration.LightColor,
                                             specularComponent, _configuration.SpecularIntensity);

                    resultingColor.a = canvasColor.a;
                    canvas.SetPixel(x, y, resultingColor);
                }
            }
        }

        private Vector3 GetNormal(Vector3 position)
        {
            var originA = new Vector3(position.x, position.y, position.z + PRECISION);
            var originB = new Vector3(position.x + PRECISION_COS, position.y, position.z - PRECISION_SIN);
            var originC = new Vector3(position.x - PRECISION_COS, position.y, position.z - PRECISION_SIN);

            if (!Physics.Raycast(originA, Vector3.down, out var hitA, RAYCAST_DISTANCE, _raycastMask,
                    QueryTriggerInteraction.Ignore) ||
                !Physics.Raycast(originB, Vector3.down, out var hitB, RAYCAST_DISTANCE, _raycastMask,
                    QueryTriggerInteraction.Ignore) ||
                !Physics.Raycast(originC, Vector3.down, out var hitC, RAYCAST_DISTANCE, _raycastMask,
                    QueryTriggerInteraction.Ignore))
                return new Vector3(0, 1, 0);

            var a = hitA.point;
            var b = hitB.point;
            var c = hitC.point;

            return Vector3.Cross(b - a, c - a).normalized;
        }

        private static float GetDiffuse(Vector3 normal, Vector3 reversedLightDirection)
        {
            return Mathf.Clamp01(Vector3.Dot(reversedLightDirection, normal));
        }

        private static float GetSpecular(Vector3 normal, Vector3 lightDirection, Vector3 reversedViewDirection)
        {
            var reflectionDirection = lightDirection - 2 * Vector3.Dot(lightDirection, normal) * normal;
            return Mathf.Clamp01(Vector3.Dot(reversedViewDirection, reflectionDirection));
        }

        private static Color GetLightComponent(Color targetColor, Color lightColor, float intensity)
        {
            return targetColor * lightColor * intensity;
        }

        private static Color GetSpecularComponent(Color specularColor, Color lightColor, float specular,
            float intensity)
        {
            return specularColor * lightColor * Mathf.Pow(specular, 4) * intensity;
        }
    }
}