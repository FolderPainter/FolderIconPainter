using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WpfClient.Models
{
    /// <summary>
    /// This class contains a folder colors properties
    /// </summary>
    public static class FolderColors
    {
        public readonly static string RedFolder = "#ff1818";
        public readonly static string Lime = "#8aff3c";
        public readonly static string Rose = "#ff29ed";
        public readonly static string BrightTurquoise = "#11f8cf";
        public readonly static string DarkTurquoise = "#29b79e";
        public readonly static string Yellow = "#ffe849";
        public readonly static string Green = "#38ae03";
        public readonly static string CornflowerBlue = "#6677f4";
        public readonly static string Orange = "#ffac5a";
        public readonly static string Meganta = "#d748fb";
        public readonly static string DarkMeganta = "#9c0597";
        public readonly static string BrightCyan = "#32ffc1";
        public readonly static string Malibu = "#74c2ff";
        public readonly static string Gray = "#B5B5B5";
        public readonly static string BrightMagenta = "#FF269E";
        public readonly static string Blue = "#0D28EE";

        public static IEnumerable<string> GetAllColors()
        {
            return typeof(FolderColors).GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .Select(f => (string)f.GetValue(null));
        }
    }
}
