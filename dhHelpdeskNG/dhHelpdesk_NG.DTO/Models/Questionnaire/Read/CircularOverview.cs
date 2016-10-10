namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CircularOverview
    {
        #region Constructors and Destructors

        #endregion

        #region Properties

        public CircularOverview(
            int id,
            string circularName,
            DateTime date,
            CircularStates state,
            int totalParticipants,
            int sentParticipants,
            int answeredParticipants)
        {
            this.Id = id;
            this.CircularName = circularName;
            this.Date = date;
            this.State = state;
            this.TotalParticipants = totalParticipants;
            this.SentParticipants = sentParticipants;
            AnsweredParticipants = answeredParticipants;
        }

        [IsId]
        public int Id { get; private set; }

        public string CircularName { get; private set; }

        public DateTime Date { get; private set; }

        public CircularStates State { get; private set; }

        public int TotalParticipants { get; private set; }

        public int SentParticipants { get; private set; }

        public int AnsweredParticipants { get; private set; }

        #endregion
    }
}