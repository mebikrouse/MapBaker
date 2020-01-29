using UnityEngine;

namespace MapBaker.Utility
{
    public static class Extensions
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, color.a * alpha);
        }
    }
}