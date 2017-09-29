using System;
using System.Collections.Generic;
using System.Drawing;

namespace SupSubtitleParser
{
    public class ImageDecoder
    {
        int pos = 0;

        public Bitmap Decode(byte[] data, Size size, List<PDSEntryObject> palettes, Color bgColor)
        {
            pos = 0;
            FastBitmap fastBitmap = new FastBitmap(size);

            for (; ; )
            {
                if (pos >= data.Length)
                {
                    break;
                }
                var ColorTimes = GetColorTimes(data);

                if (ColorTimes.colorIdx == 0 && ColorTimes.times == 0)
                {
                    continue;
                }
                else
                {
                    Color color = Color.Transparent;
                    if (ColorTimes.colorIdx == 0xff)
                    {
                        color = bgColor;
                    }
                    else
                    {
                        var rgb = YCbCr2Rgb(palettes[ColorTimes.colorIdx].Luminance,
                                palettes[ColorTimes.colorIdx].ColorDifferenceRed,
                                palettes[ColorTimes.colorIdx].ColorDifferenceBlue);

                        if (palettes[ColorTimes.colorIdx].Transparency != 255)
                        {
                            color = bgColor;
                        }
                        else
                        {
                            color = Color.FromArgb(palettes[ColorTimes.colorIdx].Transparency, rgb.r, rgb.g, rgb.b);
                        }
                    }

                    for (int j = 0; j < ColorTimes.times; j++)
                    {
                        fastBitmap.AddPixel(color);
                    }
                }
            }

            return fastBitmap.GetBitmap();
        }

        private (int colorIdx, UInt16 times) GetColorTimes(byte[] data)
        {
            byte b0 = data[pos++];
            if (b0 != 0) //CCCCCCCC
            {
                return (b0, 1);
            }

            var b1 = data[pos++];

            if (b1 == 0) //00000000 00000000
            {
                //End of Line 
                return (0, 0);
            }

            var flg = b1 & 0xC0;
            if (flg == 0) //00000000 00LLLLLL
            {
                var times = (UInt16)(b1 & 0x3f);
                return (0, times);
            }

            var b2 = data[pos++];
            if (flg == 0x40) //00000000 01LLLLLL LLLLLLLL
            {
                var times = (UInt16)(((b1 & 0x3f) << 8) + b2);
                return (0, times);
            }

            if (flg == 0x80) //00000000 10LLLLLL CCCCCCCC
            {
                var times = (UInt16)(b1 & 0x3f);
                return (b2, times);
            }

            if (flg == 0xC0) //00000000 11LLLLLL LLLLLLLL CCCCCCCC
            {
                var b3 = data[pos++];
                var times = (UInt16)(((b1 & 0x3f) << 8) + b2);
                return (b3, times);
            }

            return (0, 0);
        }

        private (int r, int g, int b) YCbCr2Rgb(int y, int cb, int cr)
        {
            double r, g, b;

            y -= 16;
            cb -= 128;
            cr -= 128;

            var y1 = y * 1.164383562;

            r = y1 + cr * 1.792741071;
            g = y1 - cr * 0.5329093286 - cb * 0.2132486143;
            b = y1 + cb * 2.112401786;

            r = r < 0 ? 0 : (r > 255 ? 255 : r);
            g = g < 0 ? 0 : (g > 255 ? 255 : g);
            b = b < 0 ? 0 : (b > 255 ? 255 : b);

            return ((int)(r + 0.5), (int)(g + 0.5), (int)(b + 0.5));
        }
    }
}
