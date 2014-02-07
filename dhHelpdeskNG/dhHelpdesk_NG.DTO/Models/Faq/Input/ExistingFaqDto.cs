namespace DH.Helpdesk.BusinessData.Models.Faq.Input
{
    using System;

    public sealed class ExistingFaqDto
    {
        #region Constructors and Destructors

        public ExistingFaqDto(
            int id,
            int faqCategoryId,
            string question,
            string answer,
            string internalAnswer,
            string urlOne,
            string urlTwo,
            int? workingGroupId,
            bool informationIsAvailableForNotifiers,
            bool showOnStartPage,
            DateTime changedDate)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("id", "Must be more than zero.");
            }

            if (faqCategoryId <= 0)
            {
                throw new ArgumentOutOfRangeException("faqCategoryId", "Must be more than zero.");
            }

            if (string.IsNullOrEmpty(question))
            {
                throw new ArgumentNullException("question", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentNullException("answer", "Value cannot be null or empty.");
            }

            this.Id = id;
            this.FaqCategoryId = faqCategoryId;
            this.Question = question;
            this.Answer = answer;
            this.InternalAnswer = internalAnswer;
            this.UrlOne = urlOne;
            this.UrlTwo = urlTwo;
            this.WorkingGroupId = workingGroupId;
            this.InformationIsAvailableForNotifiers = informationIsAvailableForNotifiers;
            this.ShowOnStartPage = showOnStartPage;
            this.ChangedDate = changedDate;
        }

        #endregion

        #region Public Properties

        public string Answer { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int FaqCategoryId { get; private set; }

        public int Id { get; private set; }

        public bool InformationIsAvailableForNotifiers { get; private set; }

        public string InternalAnswer { get; private set; }

        public string Question { get; private set; }

        public bool ShowOnStartPage { get; private set; }

        public string UrlOne { get; private set; }

        public string UrlTwo { get; private set; }

        public int? WorkingGroupId { get; private set; }

        #endregion
    }
}