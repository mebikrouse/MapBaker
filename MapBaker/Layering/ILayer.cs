using MapBaker.Bitmapping;

namespace MapBaker.Layering
{
    public interface ILayer
    {
        void Draw(IBitmap canvas);
    }
}