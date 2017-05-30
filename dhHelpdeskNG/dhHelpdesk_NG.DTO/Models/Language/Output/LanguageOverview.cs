// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageOverview.cs" company="">
//   
// </copyright>
// <summary>
//   The language overview.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Language.Output
{
    /// <summary>
    /// The language overview.
    /// </summary>
    public class LanguageOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public int IsActive { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}