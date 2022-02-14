using MudBlazor.Utilities;
using System;

namespace WebApp.Models
{
    public class FilterGenerator
    {
        public MudColor TargetColor { get; set; }

        public MudColor ReusedColor { get; set; }

        public ColorHSL TargetHSL { get; set; }

        public FilterGenerator(string targetColor, string baseColor)
        {
            TargetColor = new(targetColor);
            ReusedColor = new(baseColor);
            TargetHSL = new(TargetColor.H, TargetColor.S, TargetColor.L);
        }

        public string Generate()
        {
            return "";
        }

        public string GenerateWide()
        {
            const int A = 5;
            const int C = 15;
            float[] a = new float[] { 60f, 180f, 18000f, 600f, 1.2f, 1.2f };

            return "";
        }


    }
}
