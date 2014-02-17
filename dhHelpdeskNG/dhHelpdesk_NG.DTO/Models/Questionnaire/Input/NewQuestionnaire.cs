namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewQuestionnaire : INewBusinessModel
    {
        #region Constructors and Destructors

        public NewQuestionnaire(string name, string description, int customerId, DateTime createDate)
        {
            this.Name = name;
            this.Description = description;
            this.CustomerId = customerId;
            this.CreatedDate = createDate;
        }

        #endregion

        #region Public Properties

        public DateTime CreatedDate { get; private set; }

        public int CustomerId { get; private set; }

        public string Description { get; private set; }

        [IsId]
        public int Id { get; set; }

        public string Name { get; private set; }

        #endregion
    }
}