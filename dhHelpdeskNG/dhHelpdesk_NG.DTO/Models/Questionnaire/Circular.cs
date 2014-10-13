namespace DH.Helpdesk.BusinessData.Models.Questionnaire
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Circular : BusinessModel
    {
        protected Circular(string circularName)
        {
            this.CircularName = circularName;
        }

        [NotNullAndEmpty]
        [MaxLength(50)]
        public string CircularName { get; private set; }
    }
}