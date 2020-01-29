using System;
using UnityEngine;

namespace MapBaker.Bitmapping
{
    public class SeamlessBitmap : IBitmap, IDisposable
    {
        private readonly IBitmap _prototype;

        public int Width { get; }

        public int Height { get; }

        public SeamlessBitmap(IBitmap prototype, int width, int height)
        {
            _prototype = prototype;

            Width = width;
            Height = height;
        }

        public void SetPixel(int x, int y, Color value)
        {
            var px = x % _prototype.Width;
            var py = y % _prototype.Height;

            _prototype.SetPixel(px, py, value);
        }

        public Color GetPixel(int x, int y)
        {
            var px = x % _prototype.Width;
            var py = y % _prototype.Height;

            return _prototype.GetPixel(px, py);
        }

        public void Dispose()
        {
            (_prototype as IDisposable)?.Dispose();
        }
    }
}