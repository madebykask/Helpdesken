// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaqOverviewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the FaqOverviewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Mobile.Models.Faq.Output
{
    using System;

    using DH.Helpdesk.Common.Extensions.DateTime;

    /// <summary>
    /// The overview model.
    /// </summary>
    public class FaqOverviewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaqOverviewModel"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="createdDate">
        /// The created date.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public FaqOverviewModel(int id, DateTime createdDate, string text)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id", "Must be more than zero.");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text", "Value cannot be null or empty.");
            }

            this.Id = id;
            this.CreatedDate = createdDate;
            this.Text = text;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the created date.
        /// </summary>
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Gets the created date text.
        /// </summary>
        public string CreatedDateText
        {
            get
            {
                return this.CreatedDate.ToFormattedDate();
            }
        }
    }
}