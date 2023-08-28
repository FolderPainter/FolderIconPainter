using System;

namespace FIP.Core.Models
{
    [Serializable]
    public class CustomIcon : StorageObject, IEquatable<CustomIcon>
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string InfoTip { get; set; }

        /// <summary>
        /// HEX formatted color.
        /// </summary>
        public string Color { get; set; }

        public bool Equals(CustomIcon other) =>
            Id == other.Id &&
            CategoryId == other.CategoryId &&
            Name == other.Name &&
            InfoTip == other.InfoTip &&
            Color == other.Color;

        //public static bool operator ==(CustomIcon obj1, CustomIcon obj2)
        //{
        //    if (ReferenceEquals(obj1, obj2))
        //        return true;
        //    if (ReferenceEquals(obj1, null))
        //        return false;
        //    if (ReferenceEquals(obj2, null))
        //        return false;
        //    return obj1.Equals(obj2);
        //}

        //public static bool operator !=(CustomIcon obj1, CustomIcon obj2) => !(obj1 == obj2);
    }
}
