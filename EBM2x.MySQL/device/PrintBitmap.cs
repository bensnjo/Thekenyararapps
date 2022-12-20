using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace EBM2x.WPF.device
{
    public class PrintBitmap
    {
        // 이미지를 Bytes 배열에 담아오는 부분
        public byte[] GetLogo(Bitmap bitmap, int printWidth, int threshold)
        {
            List<byte> byteList = new List<byte>();
            BitmapData data = GetBitmapData(bitmap, printWidth, threshold);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;

            // Set line spacing
            // 1B 33 n          
            //                        (0 ≤ n ≤ 255)
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            byteList.Add(Convert.ToByte('3'));
            byteList.Add((byte)24);

            while (offset < data.Height)
            {
                // Specify bit image mode
                // 1B 2A m nL nH d1...dk
                //                        m = 0, 1, 32, 33
                //                        0 ≤ nL ≤ 255
                //                        0 ≤ nH ≤ 3
                //                        0 ≤ d ≤ 255
                //                        k = nL + nH × 256[in case of m = 0, 1]
                //                        k = (nL + nH × 256) × 3[in case of m = 32, 33]
                byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
                byteList.Add(Convert.ToByte('*'));
                byteList.Add((byte)33);
                byteList.Add(width[0]);
                byteList.Add(width[1]);

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            int i = (y * data.Width) + x;

                            bool v = false;
                            if (i < dots.Length)
                                v = dots[i];

                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }
                        byteList.Add(slice);
                    }
                }
                offset += 24;
                byteList.Add(Convert.ToByte(0x0A));
            }

            // Set line spacing
            // 1B 33 n          
            //                        (0 ≤ n ≤ 255)
            byteList.Add(Convert.ToByte(0x1B));
            byteList.Add(Convert.ToByte('3'));
            byteList.Add((byte)30);
            return byteList.ToArray();
        }

        private BitmapData GetBitmapData(Bitmap bitmap, int width, int threshold)
        {
            var index = 0;
            double multiplier = width; // 이미지 width조정
            double scale = (double)(multiplier / (double)bitmap.Width);
            int xheight = (int)(bitmap.Height * scale);
            int xwidth = (int)(bitmap.Width * scale);
            var dimensions = xwidth * xheight;
            var dots = new BitArray(dimensions);

            for (var y = 0; y < xheight; y++)
            {
                for (var x = 0; x < xwidth; x++)
                {
                    var _x = (int)(x / scale);
                    var _y = (int)(y / scale);
                    var color = bitmap.GetPixel(_x, _y);
                    var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    dots[index] = (luminance < threshold);
                    index++;
                }
            }

            return new BitmapData()
            {
                Dots = dots,
                Height = (int)(bitmap.Height * scale),
                Width = (int)(bitmap.Width * scale)
            };
        }


        private class BitmapData
        {
            public BitArray Dots
            {
                get;
                set;
            }

            public int Height
            {
                get;
                set;
            }

            public int Width
            {
                get;
                set;
            }
        }
    }
}
