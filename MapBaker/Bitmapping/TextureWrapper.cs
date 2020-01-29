using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MapBaker.Bitmapping
{
    public class TextureWrapper : IBitmap, IDisposable
    {
        private readonly Texture2D _prototype;

        public int Width => _prototype.width;

        public int Height => _prototype.height;

        public TextureWrapper(Texture2D prototype)
        {
            _prototype = prototype;
        }

        public void SetPixel(int x, int y, Color value)
        {
            _prototype.SetPixel(x, y, value);
        }

        public Color GetPixel(int x, int y)
        {
            return _prototype.GetPixel(x, y);
        }

        public void Dispose()
        {
            if (_prototype != null)
                Object.Destroy(_prototype);
        }
    }
}