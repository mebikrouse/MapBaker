using UnityEngine;

namespace MapBaker.Utility
{
    public static class TextureScale
    {
        public static void Scale(Texture2D tex, int newWidth, int newHeight)
        {
            var texColors = tex.GetPixels();
            var newColors = new Color[newWidth * newHeight];

            var ratioX = 1.0f / ((float) newWidth / (tex.width - 1));
            var ratioY = 1.0f / ((float) newHeight / (tex.height - 1));

            for (var y = 0; y < newHeight; y++)
            {
                var yFloor = (int) Mathf.Floor(y * ratioY);
                var y1 = yFloor * tex.width;
                var y2 = (yFloor + 1) * tex.height;
                var yw = y * newWidth;

                for (var x = 0; x < newWidth; x++)
                {
                    var xFloor = (int) Mathf.Floor(x * ratioX);
                    var xLerp = x * ratioX - xFloor;
                    newColors[yw + x] = ColorLerpUnclamped(
                        ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                        ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                        y * ratioY - yFloor);
                }
            }

            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();
        }

        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value,
                c1.g + (c2.g - c1.g) * value,
                c1.b + (c2.b - c1.b) * value,
                c1.a + (c2.a - c1.a) * value);
        }
    }
}