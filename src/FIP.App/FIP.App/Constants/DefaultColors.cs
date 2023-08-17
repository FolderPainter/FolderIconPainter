using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FIP.App.Constants
{
    /// <summary>
    /// Default folder colors
    /// </summary>
    public static class DefaultColors
    {
        public readonly static string Red = "#C71313";
        public readonly static string Lime = "#69D92A";
        public readonly static string Rose = "#D122C2";
        public readonly static string BrightTurquoise = "#0ECCAA";
        public readonly static string DarkTurquoise = "#239C86";
        public readonly static string Yellow = "#E0DE39";
        public readonly static string Green = "#38AE03";
        public readonly static string CornflowerBlue = "#5C6BDB";
        public readonly static string Orange = "#EB8519";
        public readonly static string Magenta = "#C943EB";
        public readonly static string DarkMagenta = "#920FA8";
        public readonly static string BrightCyan = "#2FEDB3";
        public readonly static string Malibu = "#6AB1E8";
        public readonly static string Gray = "#707070";
        public readonly static string BrightMagenta = "#FF269E";
        public readonly static string Blue = "#0D28EE";

        public static IEnumerable<string> GetAllColors()
        {
            return typeof(DefaultColors).GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .Select(f => (string)f.GetValue(null));
        }
    }
}
