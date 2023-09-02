using System.Collections;

namespace FIP.App.ViewModels
{
    /// <summary>
    /// Provides static methods for use in x:Bind function binding to convert bound values to the required value.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Returns the reverse of the provided value.
        /// </summary>
        public static bool Not(bool value) => !value;

        /// <summary>
        /// Returns true if the specified value is not null; otherwise, returns false.
        /// </summary>
        public static bool IsNotNull(object value) => value is not null;

        /// <summary>
        /// Returns true if the specified <see cref="IList"/> value is not null or empty; otherwise, returns false.
        /// </summary>
        public static bool IsNotNullOrEmpty(IList value) => !(value is null or []);
    }
}
