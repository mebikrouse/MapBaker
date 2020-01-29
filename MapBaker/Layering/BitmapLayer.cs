using System;
using MapBaker.Bitmapping;
using MapBaker.Blending;
using MapBaker.Utility.Providers;
using UnityEngine;

namespace MapBaker.Layering
{
    public class BitmapLayer : ILayer
    {
        private readonly IProvider<IBitmap> _bitmapProvider;
        private readonly IBlending<Color> _blending;

        public BitmapLayer(IProvider<IBitmap> bitmapProvider, IBlending<Color> blending)
        {
            _bitmapProvider = bitmapProvider;
            _blending = blending;
        }

        public void Draw(IBitmap canvas)
        {
            var bitmap = _bitmapProvider.GetItem();
            for (var x = 0; x < canvas.Width; x++)
            for (var y = 0; y < canvas.Height; y++)
                canvas.SetPixel(x, y, _blending.Blend(canvas.GetPixel(x, y), bitmap.GetPixel(x, y)));

            (bitmap as IDisposable)?.Dispose();
        }
    }
}