namespace dhHelpdesk_NG.DTO.DTOs.Faq.Input
{
    using System;

    public sealed class NewFaqDto : IBusinessModelWithId
    {
        #region Constructors and Destructors

        public NewFaqDto(int categoryId, string question, string answer, string internalAnswer, string urlOne, string urlTwo, int? workingGroupId, bool informationIsAvailableForNotifiers, bool showOnStartPage, int customerId, DateTime createdDate)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentOutOfRangeException("categoryId", "Must be more than zero.");
            }

            if (string.IsNullOrEmpty(question))
            {
                throw new ArgumentNullException("question", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentNullException("answer", "Value cannot be null or empty.");
            }

            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException("customerId", "Must be more than zero.");
            }

            this.CategoryId = categoryId;
            this.Question = question;
            this.Answer = answer;
            this.InternalAnswer = internalAnswer;
            this.UrlOne = urlOne;
            this.UrlTwo = urlTwo;
            this.WorkingGroupId = workingGroupId;
            this.InformationIsAvailableForNotifiers = informationIsAvailableForNotifiers;
            this.ShowOnStartPage = showOnStartPage;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
        }

        #endregion

        #region Public Properties

        public string Answer { get; private set; }

        public int CategoryId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int CustomerId { get; private set; }

        public bool InformationIsAvailableForNotifiers { get; private set; }

        public string InternalAnswer { get; private set; }

        public string Question { get; private set; }

        public bool ShowOnStartPage { get; private set; }

        public string UrlOne { get; private set; }

        public string UrlTwo { get; private set; }

        public int? WorkingGroupId { get; private set; }

        #endregion

        public int Id { get; set; }
    }
}