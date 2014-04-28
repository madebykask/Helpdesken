// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModuleOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the UserModuleOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Users.Output
{
    /// <summary>
    /// The user module overview.
    /// </summary>
    public sealed class UserModuleOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int User_Id { get; set; }

        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        public int Module_Id { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is visible.
        /// </summary>
        public bool isVisible { get; set; }

        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int? NumberOfRows { get; set; }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        public ModuleOverview Module { get; set; }
    }
}