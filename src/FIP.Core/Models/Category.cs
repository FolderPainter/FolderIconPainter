using System;

namespace FIP.Core.Models
{
    [Serializable]
    public class Category : StorageObject, IEquatable<Category>
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Category other) =>
            Id == other.Id &&
            Name == other.Name;
    }
}
