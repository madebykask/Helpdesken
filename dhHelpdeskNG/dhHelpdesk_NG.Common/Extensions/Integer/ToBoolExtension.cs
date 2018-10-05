// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToBoolExtension.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ToBoolExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Common.Extensions.Integer
{
    /// <summary>
    /// The to boolean extension.
    /// </summary>
    public static class ToBoolExtension
    {
        /// <summary>
        /// The to boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ToBool(this int value)
        {
            return value != 0;
        }


        /// <summary>
        /// The to boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ToBool(this int? value)
        {
            if (!value.HasValue)
            {
                return false;
            }

            return value.Value != 0;
        }

        public static bool ToDefaultTrueBool(this int? value)
        {
            if (!value.HasValue)
            {
                return true;
            }

            return value.Value != 0;
        }
    }
}
