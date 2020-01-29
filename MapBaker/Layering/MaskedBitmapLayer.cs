using System;
using MapBaker.Bitmapping;
using MapBaker.Blending;
using MapBaker.Masking;
using MapBaker.Utility;
using MapBaker.Utility.Providers;
using UnityEngine;

namespace MapBaker.Layering
{
    public class MaskedBitmapLayer : ILayer
    {
        private readonly IProvider<IBitmap> _bitmapProvider;
        private readonly IMask _mask;
        private readonly IBlending<Color> _blending;

        public MaskedBitmapLayer(IProvider<IBitmap> bitmapProvider, IMask mask, IBlending<Color> blending)
        {
            _bitmapProvider = bitmapProvider;
            _mask = mask;
            _blending = blending;
        }

        public void Draw(IBitmap canvas)
        {
            var bitmap = _bitmapProvider.GetItem();
            for (var x = 0; x < canvas.Width; x++)
            for (var y = 0; y < canvas.Height; y++)
                canvas.SetPixel(x, y,
                    _blending.Blend(canvas.GetPixel(x, y), bitmap.GetPixel(x, y).WithAlpha(_mask.GetOpacity(x, y))));

            (bitmap as IDisposable)?.Dispose();
        }
    }
}