using System;
using System.Collections.Generic;
using System.Text;

namespace SourceAFIS.Visualization
{
    public class PixelFormat
    {
        public static byte ToByte(float gray)
        {
            if (gray >= 1)
                return 255;
            else if (gray <= 0)
                return 0;
            else
                return (byte)Convert.ToInt32(gray * 255);
        }

        public static byte ToByte(ColorB color)
        {
            return (byte)((color.R + color.G + color.B + 1) / 3);
        }

        public static byte[,] ToByte(ColorB[,] input)
        {
            byte[,] output = new byte[input.GetLength(0), input.GetLength(1)];
            for (int y = 0; y < input.GetLength(0); ++y)
                for (int x = 0; x < input.GetLength(1); ++x)
                    output[y, x] = ToByte(input[y, x]);
            return output;
        }

        public static float ToFloat(byte gray)
        {
            return gray * (1f / 255);
        }

        public static float ToFloat(ColorF color)
        {
            return (color.R + color.G + color.B) / 3;
        }

        public static ColorB ToColorB(ColorF color)
        {
            return new ColorB(ToByte(color.R), ToByte(color.G), ToByte(color.B));
        }

        public static ColorB[,] ToColorB(ColorF[,] input)
        {
            ColorB[,] output = new ColorB[input.GetLength(0), input.GetLength(1)];
            for (int y = 0; y < input.GetLength(0); ++y)
                for (int x = 0; x < input.GetLength(1); ++x)
                    output[y, x] = ToColorB(input[y, x]);
            return output;
        }

        public static ColorF ToColorF(byte gray)
        {
            return ToColorF(ToFloat(gray));
        }

        public static ColorF[,] ToColorF(byte[,] input)
        {
            ColorF[,] output = new ColorF[input.GetLength(0), input.GetLength(1)];
            for (int y = 0; y < input.GetLength(0); ++y)
                for (int x = 0; x < input.GetLength(1); ++x)
                    output[y, x] = ToColorF(input[y, x]);
            return output;
        }

        public static ColorF ToColorF(float gray)
        {
            return new ColorF(gray, gray, gray, 1);
        }

        public static ColorF[,] ToColorF(float[,] input)
        {
            ColorF[,] output = new ColorF[input.GetLength(0), input.GetLength(1)];
            for (int y = 0; y < input.GetLength(0); ++y)
                for (int x = 0; x < input.GetLength(1); ++x)
                    output[y, x] = ToColorF(input[y, x]);
            return output;
        }

        public static ColorF ToColorF(ColorB color)
        {
            return new ColorF(ToFloat(color.R), ToFloat(color.G), ToFloat(color.B), 1);
        }

        public static ColorF[,] ToColorF(ColorB[,] input)
        {
            ColorF[,] output = new ColorF[input.GetLength(0), input.GetLength(1)];
            for (int y = 0; y < input.GetLength(0); ++y)
                for (int x = 0; x < input.GetLength(1); ++x)
                    output[y, x] = ToColorF(input[y, x]);
            return output;
        }
    }
}
