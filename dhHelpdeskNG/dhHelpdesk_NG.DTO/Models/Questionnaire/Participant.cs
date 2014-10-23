namespace DH.Helpdesk.BusinessData.Models.Questionnaire
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Participant : BusinessModel
    {
        protected Participant(Guid guid)
        {
            this.Guid = guid;
        }

        [NotNull]
        public Guid Guid { get; private set; }
    }
}