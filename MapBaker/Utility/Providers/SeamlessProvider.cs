using MapBaker.Bitmapping;

namespace MapBaker.Utility.Providers
{
    public class SeamlessProvider : IProvider<IBitmap>
    {
        private readonly IProvider<IBitmap> _prototypeProvider;
        private readonly int _width;
        private readonly int _height;

        public SeamlessProvider(IProvider<IBitmap> prototypeProvider, int width, int height)
        {
            _prototypeProvider = prototypeProvider;
            _width = width;
            _height = height;
        }

        public IBitmap GetItem()
        {
            var prototype = _prototypeProvider.GetItem();
            return new SeamlessBitmap(prototype, _width, _height);
        }
    }
}