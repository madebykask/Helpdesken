﻿namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System;

    using DH.Helpdesk.Common.Enums;

    public sealed class CircularForEdit : Circular
    {
        public CircularForEdit(
            int id,
            string circularName,
            int questionnaireId,
            CircularStates status,
            DateTime createdDate,
            DateTime changedDate)
            : base(circularName)
        {
            this.Id = id;
            this.QuestionnaireId = questionnaireId;
            this.Status = status;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public int QuestionnaireId { get; private set; }

        public CircularStates Status { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}
