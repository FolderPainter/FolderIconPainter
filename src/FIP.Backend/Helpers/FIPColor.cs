﻿using System;
using System.Globalization;

namespace FIP.Backend.Helpers
{
    public enum ColorOutputFormats
    {
        /// <summary>
        /// Output will be starting with a # and include r,g and b but no alpha values. Example #ab2a3d
        /// </summary>
        Hex,
        /// <summary>
        /// Output will be starting with a # and include r,g and b and alpha values. Example #ab2a3dff
        /// </summary>
        HexA,
        /// <summary>
        /// Will output css like output for value. Example rgb(12,15,40)
        /// </summary>
        RGB,
        /// <summary>
        /// Will output css like output for value with alpha. Example rgba(12,15,40,0.42)
        /// </summary>
        RGBA,
        /// <summary>
        /// Will output the color elements without any decorator and without alpha. Example 12,15,26
        /// </summary>
        ColorElements
    }

    public class FIPColor : IEquatable<FIPColor>
    {
        #region Fields and Properties

        private byte[] _valuesAsByte;

        public string Value => $"#{R:x2}{G:x2}{B:x2}{A:x2}";

        public byte R => _valuesAsByte[0];
        public byte G => _valuesAsByte[1];
        public byte B => _valuesAsByte[2];
        public byte A => _valuesAsByte[3];
        public double APercentage => Math.Round((A / 255.0), 2);

        public double H { get; private set; }
        public double L { get; private set; }
        public double S { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="FIPColor"/> from the specified hue, saturation, lightness, and alpha values.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range lightness</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns>The created <see cref="FIPColor"/>.</returns>
        public FIPColor(double hue, double saturation, double lightness, int alpha)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue));
            }

            _valuesAsByte = new byte[4];

            double chroma = (1 - Math.Abs((2 * lightness) - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
            double m = lightness - (0.5 * chroma);
            double r1, g1, b1;


            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            _valuesAsByte[0] = (byte)(255 * (r1 + m)); // Red
            _valuesAsByte[1] = (byte)(255 * (g1 + m)); // Green
            _valuesAsByte[2] = (byte)(255 * (b1 + m)); // Blue
            _valuesAsByte[3] = (byte)alpha; // Alpha

            H = Math.Round(hue, 0);
            S = Math.Round(saturation, 2);
            L = Math.Round(lightness, 2);
        }

        public FIPColor(byte r, byte g, byte b, byte a)
        {
            _valuesAsByte = new byte[4];

            _valuesAsByte[0] = r;
            _valuesAsByte[1] = g;
            _valuesAsByte[2] = b;
            _valuesAsByte[3] = a;

            CalculateHSL();
        }

        /// <summary>
        /// initialize a new MudColor with new RGB values but keeps the hue value from the color
        /// </summary>
        /// <param name="r">R</param>
        /// <param name="g">G</param>
        /// <param name="b">B</param>
        /// <param name="color">Existing color to copy hue value from </param>
        public FIPColor(byte r, byte g, byte b, FIPColor color) : this(r,g,b,color.A)
        {
            H = color.H;
        }

        public FIPColor(int r, int g, int b, double alpha) :
         this(r, g, b, (byte)((alpha * 255.0)))
        {

        }

        public FIPColor(int r, int g, int b, int alpha) :
            this((byte)r, (byte)g, (byte)b, (byte)alpha)
        {

        }

        public FIPColor(string value)
        {
            value = value.Trim().ToLower();

            if (value.StartsWith("rgba") == true)
            {
                var parts = SplitInputIntoParts(value);
                if (parts.Length != 4)
                {
                    throw new ArgumentException("invalid color format");
                }

                _valuesAsByte = new byte[]
                {
                    byte.Parse(parts[0],CultureInfo.InvariantCulture),
                    byte.Parse(parts[1],CultureInfo.InvariantCulture),
                    byte.Parse(parts[2],CultureInfo.InvariantCulture),
                    (byte)Math.Max(0, Math.Min(255, 255 * double.Parse(parts[3],CultureInfo.InvariantCulture))),
                };
            }
            else if (value.StartsWith("rgb") == true)
            {
                var parts = SplitInputIntoParts(value);
                if (parts.Length != 3)
                {
                    throw new ArgumentException("invalid color format");
                }
                _valuesAsByte = new byte[]
                {
                    byte.Parse(parts[0],CultureInfo.InvariantCulture),
                    byte.Parse(parts[1],CultureInfo.InvariantCulture),
                    byte.Parse(parts[2],CultureInfo.InvariantCulture),
                    255
                };
            }
            else
            {

                if (value.StartsWith("#"))
                {
                    value = value.Substring(1);
                }

                switch (value.Length)
                {
                    case 3:
                        value = new string(new char[8] { value[0], value[0], value[1], value[1], value[2], value[2], 'F', 'F' });
                        break;
                    case 4:
                        value = new string(new char[8] { value[0], value[0], value[1], value[1], value[2], value[2], value[3], value[3] });
                        break;
                    case 6:
                        value += "FF";
                        break;
                    case 8:
                        break;
                    default:
                        throw new ArgumentException("not a valid color", nameof(value));
                }

                _valuesAsByte = new byte[]
                {
                    GetByteFromValuePart(value,0),
                    GetByteFromValuePart(value,2),
                    GetByteFromValuePart(value,4),
                    GetByteFromValuePart(value,6),
                };

                CalculateHSL();
            }
        }

        #endregion

        #region Methods

        private void CalculateHSL()
        {
            // normalize red, green, blue values
            var r = R / 255D;
            var g = G / 255D;
            var b = B / 255D;

            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var chroma = max - min;
            double h1;

            // Hue calculation:
            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                // The % operator doesn't do proper modulo on negative
                // numbers, so we'll add 6 before using it
                h1 = (((g - b) / chroma) + 6) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + ((b - r) / chroma);
            }
            else
            {
                h1 = 4 + ((r - g) / chroma);
            }

            double lightness = (max + min) / 2D;
            double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs((2 * lightness) - 1));

            H = 60 * h1;
            S = saturation;
            L = lightness;
        }

        public FIPColor SetH(double h) => new(h, S, L, A);
        public FIPColor SetS(double s) => new(H, s, L, A);
        public FIPColor SetL(double l) => new(H, S, l, A);

        public FIPColor SetR(int r) => new(r, G, B, A);
        public FIPColor SetG(int g) => new(R, g, B, A);
        public FIPColor SetB(int b) => new(R, G, b, A);

        public FIPColor SetAlpha(int a) => new(R, G, B, a);
        public FIPColor SetAlpha(double a) => new(R, G, B, a);

        public FIPColor ChangeLightness(double amount) => new(H, S, Math.Max(0, Math.Min(1, L + amount)), A);
        public FIPColor ColorLighten(double amount) => ChangeLightness(+amount);
        public FIPColor ColorDarken(double amount) => ChangeLightness(-amount);
        public FIPColor ColorRgbLighten() => ColorLighten(0.075);
        public FIPColor ColorRgbDarken() => ColorDarken(0.075);

        #endregion*

        #region Helper

        private static string[] SplitInputIntoParts(string value)
        {
            var startIndex = value.IndexOf('(');
            var lastIndex = value.LastIndexOf(')');
            var subString = value[(startIndex + 1)..lastIndex];
            var parts = subString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            return parts;
        }

        private byte GetByteFromValuePart(string input, int index) => byte.Parse(new string(new char[] { input[index], input[index + 1] }), NumberStyles.HexNumber);

        public bool HslChanged(FIPColor value) => this.H != value.H || this.S != value.S || this.L != value.L;

        #endregion

        #region operators and object members

        public static implicit operator FIPColor(string input) => new(input);
        //public static implicit operator string(MudColor input) => input == null ? string.Empty : input.Value;

        public static explicit operator string(FIPColor color) => color == null ? string.Empty : color.Value;

        public override string ToString() => ToString(ColorOutputFormats.HexA);

        public string ToString(ColorOutputFormats format) => format switch
        {
            ColorOutputFormats.Hex => Value.Substring(0, 7),
            ColorOutputFormats.HexA => Value,
            ColorOutputFormats.RGB => $"rgb({R},{G},{B})",
            ColorOutputFormats.RGBA => $"rgba({R},{G},{B},{(A / 255.0).ToString(CultureInfo.InvariantCulture)})",
            ColorOutputFormats.ColorElements => $"{R},{G},{B}",
            _ => Value,
        };

        public override bool Equals(object obj) => obj is FIPColor color && Equals(color);

        public bool Equals(FIPColor other)
        {
            if (ReferenceEquals(other, null) == true) { return false; }

            return
                _valuesAsByte[0] == other._valuesAsByte[0] &&
                _valuesAsByte[1] == other._valuesAsByte[1] &&
                _valuesAsByte[2] == other._valuesAsByte[2] &&
                _valuesAsByte[3] == other._valuesAsByte[3];
        }

        public override int GetHashCode() => _valuesAsByte[0] + _valuesAsByte[1] + _valuesAsByte[2] + _valuesAsByte[3];

        public static bool operator == (FIPColor lhs, FIPColor rhs)
        {
            var lhsIsNull = ReferenceEquals(null, lhs);
            var rhsIsNull = ReferenceEquals(null, rhs);
            if (lhsIsNull == true && rhsIsNull == true)
            {
                return true;
            }
            else
            {
                if ((lhsIsNull || rhsIsNull) == true)
                {
                    return false;
                }
                else
                {
                    return lhs.Equals(rhs);
                }
            }
        }

        public static bool operator != (FIPColor lhs, FIPColor rhs) => !(lhs == rhs);

        #endregion
    }
}