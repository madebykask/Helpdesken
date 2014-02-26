using System;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CircularOverview
    {
        #region Constructors and Destructors

        public CircularOverview(int id, string circularName, DateTime date, int state)
        {
            this.Id = id;
            this.CircularName = circularName;
            this.Date = date;
            this.State = state;
        }

        #endregion

        #region Properties

        [IsId]
        public int Id { get; private set; }

        public string CircularName { get; private set; }

        public DateTime Date {get; private set; }

        public int State { get; private set; }

        #endregion
    }
}