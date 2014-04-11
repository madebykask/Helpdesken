// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ChangeOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Change
{
    /// <summary>
    /// The change overview.
    /// </summary>
    public sealed class ChangeOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the change title.
        /// </summary>
        public string ChangeTitle { get; set; }
    }
}