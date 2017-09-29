using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SupSubtitleParser
{
    public class FastBitmap
    {
        private Bitmap _bmp = null;
        private BitmapData _bmpData = null;
        private IntPtr _ptr = new IntPtr(0);

        public FastBitmap(Bitmap bmp)
        {
            _bmp = bmp;
        }

        public FastBitmap(Size size)
        {
            _bmp = new Bitmap(size.Width, size.Height);
        }

        public FastBitmap(int width, int height)
        {
            _bmp = new Bitmap(width, height);
        }

        private void LockBits()
        {
            _bmpData = _bmp.LockBits(
                        new Rectangle(0, 0, _bmp.Width, _bmp.Height),
                        ImageLockMode.ReadWrite,
                        PixelFormat.Format32bppArgb);
            _ptr = _bmpData.Scan0;
        }

        public void AddPixel(Color color)
        {
            if (_bmpData == null)
            {
                LockBits();
            }

            byte[] byt = new byte[] { color.R, color.G, color.B, color.A };
            Marshal.Copy(byt, 0, _ptr, 4);
            _ptr += 4;
        }

        public void Unlock()
        {
            if (_bmpData != null)
            {
                _bmp.UnlockBits(_bmpData);
                _bmpData = null;
            }
        }

        public Bitmap GetBitmap()
        {
            Unlock();
            return _bmp;
        }

        public void SaveAs(string fileName)
        {
            Unlock();

            ImageFormat fmt = ImageFormat.Bmp;

            if (fileName.ToLower().EndsWith(".jpg"))
            {
                fmt = ImageFormat.Jpeg;
            }
            else if (fileName.ToLower().EndsWith(".png"))
            {
                fmt = ImageFormat.Png;
            }
            else if (fileName.ToLower().EndsWith(".gif"))
            {
                fmt = ImageFormat.Gif;
            }
            else if (fileName.ToLower().EndsWith(".tiff") || fileName.ToLower().EndsWith(".tif"))
            {
                fmt = ImageFormat.Tiff;
            }

            _bmp.Save(fileName, fmt);
        }
    }
}
