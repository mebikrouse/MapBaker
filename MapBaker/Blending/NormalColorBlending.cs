using System;
using UnityEngine;

namespace MapBaker.Blending
{
    public class NormalColorBlending : IBlending<Color>
    {
        public static NormalColorBlending Instance { get; } = new NormalColorBlending();

        public Color Blend(Color target, Color overlay)
        {
            if (Math.Abs(target.a) < float.Epsilon && Math.Abs(overlay.a) < float.Epsilon) return target;

            var a = (1 - overlay.a) * target.a + overlay.a;

            var r = ((1 - overlay.a) * target.a * target.r + overlay.a * overlay.r) / a;
            var g = ((1 - overlay.a) * target.a * target.g + overlay.a * overlay.g) / a;
            var b = ((1 - overlay.a) * target.a * target.b + overlay.a * overlay.b) / a;

            return new Color(r, g, b, a);
        }
    }
}