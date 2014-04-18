// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the StringExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Common.Extensions.String
{
    /// <summary>
    /// The string extension.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// The contain text.
        /// </summary>
        /// <param name="str">
        /// The string.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ContainsText(this string str, string text)
        {
            if (str == null || text == null)
            {
                return str == text;
            }

            return str.Trim().ToLower().Contains(text.Trim().ToLower());
        }
    }
}