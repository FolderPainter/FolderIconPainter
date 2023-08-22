using System.Collections.Generic;
using System.Globalization;
using FIP.Core.Models;
using FluentAssertions;
using Xunit;

namespace MudBlazor.UnitTests.Utilities
{
    public class FIPColorTests
    {
        [Theory, MemberData(nameof(TestData_Create_By_Hex_String))]
        public void Create_By_Hex_String(string input, byte r, byte g, byte b, byte alpha)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(input);

                color.R.Should().Be(r);
                color.G.Should().Be(g);
                color.B.Should().Be(b);
                color.A.Should().Be(alpha);

                FIPColor implicitCasted = input;

                implicitCasted.R.Should().Be(r);
                implicitCasted.G.Should().Be(g);
                implicitCasted.B.Should().Be(b);
                implicitCasted.A.Should().Be(alpha);
            }
        }

        public static IEnumerable<object[]> TestData_Create_By_Hex_String()
        {
            yield return WrapArgs("12315aca", 18, 49, 90, 202);
            yield return WrapArgs("12315a", 18, 49, 90, 255);
            yield return WrapArgs("#12315a", 18, 49, 90, 255);
            yield return WrapArgs("12315ACA", 18, 49, 90, 202);
            yield return WrapArgs("12315Aca", 18, 49, 90, 202);
            yield return WrapArgs("#12315Aca", 18, 49, 90, 202);
            yield return WrapArgs("1ab", 17, 170, 187, 255);
            yield return WrapArgs("1AB", 17, 170, 187, 255);
            yield return WrapArgs("1abd", 17, 170, 187, 221);

            static object[] WrapArgs(
                string hexString,
                byte r,
                byte g,
                byte b,
                byte alpha)
                => new object[]
                {
                    hexString,
                    r,
                    g,
                    b,
                    alpha
                };
        }

        [Theory, MemberData(nameof(TestData_Create_By_RGB_String))]
        public void Create_By_RGB_String(string input, byte r, byte g, byte b, byte alpha)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(input);

                color.R.Should().Be(r);
                color.G.Should().Be(g);
                color.B.Should().Be(b);
                color.A.Should().Be(alpha);

                FIPColor implicitCasted = input;

                implicitCasted.R.Should().Be(r);
                implicitCasted.G.Should().Be(g);
                implicitCasted.B.Should().Be(b);
                implicitCasted.A.Should().Be(alpha);
            }
        }

        public static IEnumerable<object[]> TestData_Create_By_RGB_String()
        {
            yield return WrapArgs("rgb(12,204,210)", 12, 204, 210, 255);
            yield return WrapArgs("rgb(0,0,0)", 0, 0, 0, 255);
            yield return WrapArgs("rgb(255,255,255)", 255, 255, 255, 255);

            static object[] WrapArgs(
                string rgbString,
                byte r,
                byte g,
                byte b,
                byte alpha)
                => new object[]
                {
                    rgbString,
                    r,
                    g,
                    b,
                    alpha
                };
        }

        [Theory, MemberData(nameof(TestData_Create_By_RGBA_String))]
        public void Create_By_RGBA_String(string input, byte r, byte g, byte b, byte alpha)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(input);

                color.R.Should().Be(r);
                color.G.Should().Be(g);
                color.B.Should().Be(b);
                color.A.Should().Be(alpha);

                FIPColor implicitCasted = input;

                implicitCasted.R.Should().Be(r);
                implicitCasted.G.Should().Be(g);
                implicitCasted.B.Should().Be(b);
                implicitCasted.A.Should().Be(alpha);
            }
        }

        public static IEnumerable<object[]> TestData_Create_By_RGBA_String()
        {
            yield return WrapArgs("rgba(12,204,210,0.5)", 12, 204, 210, 127);
            yield return WrapArgs("rgba(0,0,0,0)", 0, 0, 0, 0);
            yield return WrapArgs("rgba(255,255,255,1)", 255, 255, 255, 255);

            static object[] WrapArgs(
                string rgbaString,
                byte r,
                byte g,
                byte b,
                byte alpha)
                => new object[]
                {
                    rgbaString,
                    r,
                    g,
                    b,
                    alpha
                };
        }

        [Fact]
        public void FromRGB_Byte()
        {
            FIPColor color = new((byte)123, (byte)240, (byte)130, (byte)76);

            color.R.Should().Be(123);
            color.G.Should().Be(240);
            color.B.Should().Be(130);
            color.A.Should().Be(76);
        }

        [Fact]
        public void FromRGB_Byte_AndAlphaDouble()
        {
            FIPColor color = new((byte)123, (byte)240, (byte)130, 0.8);

            color.R.Should().Be(123);
            color.G.Should().Be(240);
            color.B.Should().Be(130);
            color.A.Should().Be(204);
        }

        [Fact]
        public void FromRGB_Int()
        {
            FIPColor color = new((int)123, (int)240, (int)130, (int)76);

            color.R.Should().Be(123);
            color.G.Should().Be(240);
            color.B.Should().Be(130);
            color.A.Should().Be(76);
        }

        [Fact]
        public void FromRGB_Int_CapsToMaximum()
        {
            FIPColor color = new((int)300, (int)2152525, (int)266, (int)25555);

            color.R.Should().Be(255);
            color.G.Should().Be(255);
            color.B.Should().Be(255);
            color.A.Should().Be(255);
        }

        [Fact]
        public void FromRGB_Int_EnsureMinimum()
        {
            FIPColor color = new((int)-300, (int)-2152525, (int)-266, (int)-25555);

            color.R.Should().Be(0);
            color.G.Should().Be(0);
            color.B.Should().Be(0);
            color.A.Should().Be(0);
        }

        [Fact]
        public void FromRGB_Int_WithDoubleAlpha()
        {
            FIPColor color = new((int)123, (int)240, (int)130, 0.8);

            color.R.Should().Be(123);
            color.G.Should().Be(240);
            color.B.Should().Be(130);
            color.A.Should().Be(204);
        }

        [Fact]
        public void FromRGB_Int_WithDoubleAlpha_CapsToMaximum()
        {
            FIPColor color = new((int)300, (int)2152525, (int)266, 2.4);

            color.R.Should().Be(255);
            color.G.Should().Be(255);
            color.B.Should().Be(255);
            color.A.Should().Be(255);
        }

        [Fact]
        public void FromRGB_Int_WithDoubleAlpha_EnsureMinimum()
        {
            FIPColor color = new((int)-300, (int)-2152525, (int)-266, -0.8);

            color.R.Should().Be(0);
            color.G.Should().Be(0);
            color.B.Should().Be(0);
            color.A.Should().Be(0);
        }

        [Fact]
        public void FromHLS_AlphaAsInt()
        {
            FIPColor color = new(113.2424, 0.624, 0.2922525, 115);

            color.H.Should().Be(113.0);
            color.S.Should().Be(0.62);
            color.L.Should().Be(0.29);

            color.R.Should().Be(39);
            color.G.Should().Be(120);
            color.B.Should().Be(28);

            color.A.Should().Be(115);
        }

        [Fact]
        public void FromHLS_AlphaAsInt_CapsToMaximum()
        {
            FIPColor color = new(450.0, 1.4, 1.2, 266);

            color.H.Should().Be(360);
            color.S.Should().Be(1);
            color.L.Should().Be(1);

            color.A.Should().Be(255);
        }

        [Fact]
        public void FromHLS_AlphaAsInt_EnsureMinimum()
        {
            FIPColor color = new(-450.0, -1.4, -1.2, -266);

            color.H.Should().Be(0);
            color.S.Should().Be(0);
            color.L.Should().Be(0);

            color.A.Should().Be(0);
        }

        [Fact]
        public void FromHLS_AlphaAsDouble_CapsToMaximum()
        {
            FIPColor color = new(450.0, 1.4, 1.2, 1.2);

            color.H.Should().Be(360);
            color.S.Should().Be(1);
            color.L.Should().Be(1);

            color.A.Should().Be(255);
        }

        [Fact]
        public void FromHLS_AlphaAsDouble_EnsureMinimum()
        {
            FIPColor color = new(-450.0, -1.4, -1.2, -1.2);

            color.H.Should().Be(0);
            color.S.Should().Be(0);
            color.L.Should().Be(0);

            color.A.Should().Be(0);
        }

        [Theory, MemberData(nameof(TestData_Transform_RGB_To_HSL))]
        public void Transform_RGB_To_HSL(byte r, byte g, byte b, byte a, double expectedH, double expectedS, double expectedL)
        {
            FIPColor color = new(r, g, b, a);

            color.R.Should().Be(r);
            color.G.Should().Be(g);
            color.B.Should().Be(b);
            color.A.Should().Be(a);

            color.H.Should().Be(expectedH);
            color.S.Should().Be(expectedS);
            color.L.Should().Be(expectedL);
        }

        public static IEnumerable<object[]> TestData_Transform_RGB_To_HSL()
        {
            yield return WrapArgs(130, 150, 240, 130, 229, 0.79, 0.73);
            yield return WrapArgs(71, 88, 99, 222, 204, 0.16, 0.33);

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                double expectedH,
                double expectedS,
                double expectedL)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedH,
                    expectedS,
                    expectedL
                };
        }

        [Fact]
        public void SetH()
        {
            FIPColor color = new(120, 0.15, 0.25, 255);

            color.SetH(-12).H.Should().Be(0);
            color.SetH(0).H.Should().Be(0);
            color.SetH(120).H.Should().Be(120);
            color.SetH(350).H.Should().Be(350);
            color.SetH(370).H.Should().Be(360);
        }

        [Fact]
        public void SetS()
        {
            FIPColor color = new(120, 0.15, 0.25, 255);

            color.SetS(-0.1).S.Should().Be(0);
            color.SetS(0).S.Should().Be(0);
            color.SetS(0.37).S.Should().Be(0.37);
            color.SetS(0.67).S.Should().Be(0.67);
            color.SetS(1.2).S.Should().Be(1);
        }

        [Fact]
        public void SetL()
        {
            FIPColor color = new(120, 0.15, 0.25, 255);

            color.SetL(-0.1).L.Should().Be(0);
            color.SetL(0).L.Should().Be(0);
            color.SetL(0.37).L.Should().Be(0.37);
            color.SetL(0.67).L.Should().Be(0.67);
            color.SetL(1.2).L.Should().Be(1);
        }

        [Fact]
        public void SetR()
        {
            FIPColor color = new((byte)25, (byte)50, (byte)70, (byte)255);

            color.SetR(-4).R.Should().Be(0);
            color.SetR(0).R.Should().Be(0);
            color.SetR(20).R.Should().Be(20);
            color.SetR(250).R.Should().Be(250);
            color.SetR(256).R.Should().Be(255);
        }

        [Fact]
        public void SetG()
        {
            FIPColor color = new((byte)25, (byte)50, (byte)70, (byte)255);

            color.SetG(-4).G.Should().Be(0);
            color.SetG(0).G.Should().Be(0);
            color.SetG(20).G.Should().Be(20);
            color.SetG(250).G.Should().Be(250);
            color.SetG(256).G.Should().Be(255);
        }

        [Fact]
        public void SetB()
        {
            FIPColor color = new((byte)25, (byte)50, (byte)70, (byte)255);

            color.SetB(-4).B.Should().Be(0);
            color.SetB(0).B.Should().Be(0);
            color.SetB(20).B.Should().Be(20);
            color.SetB(250).B.Should().Be(250);
            color.SetB(256).B.Should().Be(255);
        }

        [Fact]
        public void SetAlpha_Byte()
        {
            FIPColor color = new((byte)25, (byte)50, (byte)70, (byte)170);

            color.SetAlpha(-4).A.Should().Be(0);
            color.SetAlpha(0).A.Should().Be(0);
            color.SetAlpha(20).A.Should().Be(20);
            color.SetAlpha(250).A.Should().Be(250);
            color.SetAlpha(256).A.Should().Be(255);
        }

        [Fact]
        public void SetAlpha_Double()
        {
            FIPColor color = new((byte)25, (byte)50, (byte)70, (byte)170);

            color.SetAlpha(-0.4).A.Should().Be(0);
            color.SetAlpha(0.0).A.Should().Be(0);
            color.SetAlpha(0.4).A.Should().Be(102);
            color.SetAlpha(0.8).A.Should().Be(204);
            color.SetAlpha(1.2).A.Should().Be(255);
        }

        [Fact]
        public void ChangeLightness()
        {
            FIPColor color = new(140.0, 0.2, 0.4, (byte)170);

            color.ChangeLightness(-0.4).L.Should().Be(0.0);
            color.ChangeLightness(-0.5).L.Should().Be(0.0);
            color.ChangeLightness(+0.5).L.Should().Be(0.9);
            color.ChangeLightness(+0.6).L.Should().Be(1.0);
            color.ChangeLightness(+0.7).L.Should().Be(1.0);
            color.ChangeLightness(+2.7).L.Should().Be(1.0);
        }

        [Fact]
        public void ColorLighten()
        {
            FIPColor color = new(140.0, 0.2, 0.4, (byte)170);

            color.ChangeLightness(0.4).L.Should().Be(0.8);
            color.ChangeLightness(0.5).L.Should().Be(0.9);
            color.ChangeLightness(0.6).L.Should().Be(1.0);
            color.ChangeLightness(0.7).L.Should().Be(1.0);
            color.ChangeLightness(-0.4).L.Should().Be(0.0);
            color.ChangeLightness(-0.5).L.Should().Be(0.0);
        }

        [Fact]
        public void ColorDarken()
        {
            FIPColor color = new(140.0, 0.2, 0.4, (byte)170);

            color.ColorDarken(0.4).L.Should().Be(0.0);
            color.ColorDarken(0.5).L.Should().Be(0.0);
            color.ColorDarken(0.2).L.Should().Be(0.2);
            color.ColorDarken(-0.6).L.Should().Be(1.0);
            color.ColorDarken(-0.7).L.Should().Be(1.0);
        }

        [Fact]
        public void ColorRgbLighten()
        {
            FIPColor color = new(140.0, 0.2, 0.5, (byte)170);
            color.ColorRgbLighten().L.Should().Be(0.57);
        }

        [Fact]
        public void ColorRgbDarken()
        {
            FIPColor color = new(140.0, 0.2, 0.5, (byte)170);
            color.ColorRgbDarken().L.Should().Be(0.42);
        }

        [Theory, MemberData(nameof(TestData_ValueAndExplicitCast))]
        public void ValueAndExplicitCast(byte r, byte g, byte b, byte a, string expectedValue)
        {
            FIPColor color = new(r, g, b, a);

            color.Value.ToLowerInvariant().Should().Be(expectedValue);
            color.ToString(ColorOutputFormats.HexA).ToLowerInvariant().Should().Be(expectedValue);
            ((string)color).ToLowerInvariant().Should().Be(expectedValue);
        }

        public static IEnumerable<object[]> TestData_ValueAndExplicitCast()
        {
            yield return WrapArgs(130, 150, 240, 170, "#8296f0aa");
            yield return WrapArgs(71, 88, 99, 204, "#475863cc");

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                string expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

        [Theory, MemberData(nameof(TestData_ToRGB))]
        public void ToRGB(byte r, byte g, byte b, byte a, string expectedValue)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(r, g, b, a);

                color.ToString(ColorOutputFormats.RGB).Should().Be(expectedValue);
            }
        }

        public static IEnumerable<object[]> TestData_ToRGB()
        {
            yield return WrapArgs(130, 150, 240, 255, "rgb(130,150,240)");
            yield return WrapArgs(71, 88, 99, 255, "rgb(71,88,99)");

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                string expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

        [Theory, MemberData(nameof(TestData_ToRGBA))]
        public void ToRGBA(byte r, byte g, byte b, byte a, string expectedValue)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(r, g, b, a);

                color.ToString(ColorOutputFormats.RGBA).Should().Be(expectedValue);
            }
        }

        public static IEnumerable<object[]> TestData_ToRGBA()
        {
            yield return WrapArgs(130, 150, 240, 255, "rgba(130,150,240,1)");
            yield return WrapArgs(71, 88, 99, 0, "rgba(71,88,99,0)");
            yield return WrapArgs(71, 88, 99, 204, "rgba(71,88,99,0.8)");

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                string expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

        [Theory, MemberData(nameof(TestData_ToColorRgbElements))]
        public void ToColorRgbElements(byte r, byte g, byte b, byte a, string expectedValue)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(r, g, b, a);

                color.ToString(ColorOutputFormats.ColorElements).Should().Be(expectedValue);
            }
        }

        public static IEnumerable<object[]> TestData_ToColorRgbElements()
        {
            yield return WrapArgs(130, 150, 240, 255, "130,150,240");
            yield return WrapArgs(71, 88, 99, 255, "71,88,99");

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                string expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

        [Theory, MemberData(nameof(TestData_ToHex))]
        public void ToHex(byte r, byte g, byte b, byte a, string expectedValue)
        {
            var cultures = new[] { new CultureInfo("en", false), new CultureInfo("se", false) };

            foreach (var item in cultures)
            {
                CultureInfo.CurrentCulture = item;

                FIPColor color = new(r, g, b, a);

                color.ToString(ColorOutputFormats.Hex).Should().Be(expectedValue);
            }
        }

        public static IEnumerable<object[]> TestData_ToHex()
        {
            yield return WrapArgs(130, 150, 240, 170, "#8296f0");
            yield return WrapArgs(71, 88, 99, 204, "#475863");

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                string expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

#pragma warning disable CS1718 // Comparison made to same variable

        [Fact]
        public void Equals_SameType()
        {
            FIPColor color1 = new(10, 20, 50, 255);
            FIPColor color2 = new(10, 20, 50, 255);

            (color1 == color1).Should().BeTrue();
            (color2 == color2).Should().BeTrue();
            (color1 == color2).Should().BeTrue();
            (color2 == color1).Should().BeTrue();

            color1.Equals(color1).Should().BeTrue();
            color2.Equals(color2).Should().BeTrue();
            color1.Equals(color2).Should().BeTrue();
            color2.Equals(color1).Should().BeTrue();
        }

        [Fact]
        public void NotEquals_SameType()
        {
            FIPColor color1 = new(10, 20, 50, 255);
            FIPColor color2 = new(10, 20, 50, 10);

            (color1 != color2).Should().BeTrue();
            (color2 != color1).Should().BeTrue();

            color1.Equals(color2).Should().BeFalse();
            color2.Equals(color1).Should().BeFalse();
        }

#pragma warning restore CS1718 // Comparison made to same variable

        [Fact]
        public void Equals_null()
        {
            FIPColor color1 = new(10, 20, 50, 255);
            (color1 == null).Should().BeFalse();
            color1.Equals(null as FIPColor).Should().BeFalse();

            FIPColor color2 = null;

            (color2 == null).Should().BeTrue();
            (null == color2).Should().BeTrue();
        }

        [Fact]
        public void Equals_DifferentObjectType()
        {
            FIPColor color1 = new(10, 20, 50, 255);
            color1.Equals(124).Should().BeFalse();
        }

        [Theory, MemberData(nameof(TestData_Get_Hash_Code))]
        public void Get_Hash_Code(byte r, byte g, byte b, byte a, int expectedValue)
        {
            FIPColor color = new(r, g, b, a);

            color.GetHashCode().Should().Be(expectedValue);
        }

        public static IEnumerable<object[]> TestData_Get_Hash_Code()
        {
            yield return WrapArgs(130, 150, 240, 255, 775);
            yield return WrapArgs(71, 88, 99, 100, 358);

            static object[] WrapArgs(
                byte red,
                byte green,
                byte blue,
                byte alpha,
                int expectedValue)
                => new object[]
                {
                    red,
                    green,
                    blue,
                    alpha,
                    expectedValue
                };
        }

        [Fact]
        public void HLSChanged_HChanged()
        {
            FIPColor first = new(120, 0.5, 0.4, 1);
            FIPColor second = new(121, 0.5, 0.4, 1);

            first.HslChanged(second).Should().BeTrue();
            second.HslChanged(first).Should().BeTrue();

            first.HslChanged(first).Should().BeFalse();
            second.HslChanged(second).Should().BeFalse();
        }

        [Fact]
        public void HLSChanged_SChanged()
        {
            FIPColor first = new(120, 0.5, 0.4, 1);
            FIPColor second = new(120, 0.51, 0.4, 1);

            first.HslChanged(second).Should().BeTrue();
            second.HslChanged(first).Should().BeTrue();

            first.HslChanged(first).Should().BeFalse();
            second.HslChanged(second).Should().BeFalse();
        }

        [Fact]
        public void HLSChanged_LChanged()
        {
            FIPColor first = new(120, 0.5, 0.4, 1);
            FIPColor second = new(120, 0.5, 0.41, 1);

            first.HslChanged(second).Should().BeTrue();
            second.HslChanged(first).Should().BeTrue();

            first.HslChanged(first).Should().BeFalse();
            second.HslChanged(second).Should().BeFalse();
        }
    }
}
