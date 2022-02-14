using System.Globalization;

namespace WebApp.Models
{
    public class ColorHSL
    {
        public ColorHSL()
        {
            H = S = L = 0;
        }

        public ColorHSL(double h, double s, double l)
        {
            H = h;
            S = s;
            L = l;
        }

        public double H { get; set; }
        public double S { get; set; }
        public double L { get; set; }

        public string HStr => H.ToString(CultureInfo.InvariantCulture);
        public string SStr => S.ToString(CultureInfo.InvariantCulture); 
        public string LStr => L.ToString(CultureInfo.InvariantCulture);

        public override string ToString()
        {
            return $"H: {HStr}   S: {SStr}   l: {LStr}";
        }
    }
}
