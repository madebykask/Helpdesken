namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.DisplayValues.Questionnaire;

    public sealed class CircularOverviewModel
    {
        #region Constructors and Destructors

        #endregion

        #region Properties

        public CircularOverviewModel(
            int id,
            string circularName,
            DateTime date,
            CircularStates state,
            int totalParticipants,
            int sentParticipants)
        {
            this.Id = id;
            this.CircularName = circularName;
            this.Date = date;
            this.State = (CircularStatesDisplayValue)state;
            this.TotalParticipants = totalParticipants;
            this.SentParticipants = sentParticipants;
        }

        [IsId]
        public int Id { get; set; }

        public string CircularName { get; set; }

        public DateTime Date { get; set; }

        public CircularStatesDisplayValue State { get; set; }

        public int TotalParticipants { get; set; }

        public int SentParticipants { get; set; }

        #endregion
    }
}