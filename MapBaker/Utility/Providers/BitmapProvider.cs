using System.IO;
using MapBaker.Bitmapping;
using UnityEngine;

namespace MapBaker.Utility.Providers
{
    public class BitmapProvider : IProvider<IBitmap>
    {
        private readonly string _path;

        public BitmapProvider(string path)
        {
            _path = path;
        }

        public IBitmap GetItem()
        {
            var texture = new Texture2D(2, 2);
            texture.LoadImage(File.ReadAllBytes(_path));

            return new TextureWrapper(texture);
        }
    }
}