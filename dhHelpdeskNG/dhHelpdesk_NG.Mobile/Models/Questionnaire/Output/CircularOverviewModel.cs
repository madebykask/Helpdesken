using System;

namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CircularOverviewModel
    {
        #region Constructors and Destructors

        public CircularOverviewModel(int id, string circularName, DateTime date, string state)
        {
            this.Id = id;
            this.CircularName = circularName;
            this.Date = date;
            this.State = state;
        }

        #endregion

        #region Properties

        [IsId]
        public int Id { get; set; }

        public string CircularName { get; set; }

        public DateTime Date {get; set; }

        public string State { get; set; }

        #endregion
    }
}