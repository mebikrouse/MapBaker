using UnityEngine;

namespace MapBaker.Bitmapping
{
    public interface IBitmap
    {
        int Width { get; }
        int Height { get; }

        void SetPixel(int x, int y, Color value);
        Color GetPixel(int x, int y);
    }
}