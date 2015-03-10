// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HolidayHeaderOverview.cs" company="">
//   
// </copyright>
// <summary>
//   The holiday header overview.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace DH.Helpdesk.BusinessData.Models.Holiday.Output
{
    /// <summary>
    /// The holiday header overview.
    /// </summary>
    public class HolidayHeaderOverview
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }  
       
        /// <summary>
        /// ID of the Department
        /// </summary>
        public int DeptartmentId { get; set; }
    }
}