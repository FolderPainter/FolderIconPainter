using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIP.App.Models
{
    public class Folder : IEquatable<Folder>
    {
        public string Name { get; set; }

        /// <summary>
        /// HEX formatted color.
        /// </summary>
        public string Color { get; set; }

        public bool Equals(Folder other) =>
            Name == other.Name &&
            Color == other.Color;
    }
}
