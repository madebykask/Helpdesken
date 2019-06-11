// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaqOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the FaqOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    /// <summary>
    ///     The overview.
    /// </summary>
    public class FaqOverview
    {
        [IsId]
        public int Id { get; set; }
       
        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }
        
        public string Text { get; set; }

        public string CustomerName { get; set; }

        public bool ShowOnStartPage { get; set; }
      
        public string CreatedDateText => CreatedDate.ToFormattedDate();

        public string ChangedDateText => ChangedDate.ToFormattedDate();
    }
}