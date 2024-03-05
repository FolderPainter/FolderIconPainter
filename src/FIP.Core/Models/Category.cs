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
        public override bool Equals(object obj)
        {
            return Equals(obj as Category);
        }

        public bool Equals(Category other) =>
            other != null &&
            Id == other.Id &&
            Name == other.Name;

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
