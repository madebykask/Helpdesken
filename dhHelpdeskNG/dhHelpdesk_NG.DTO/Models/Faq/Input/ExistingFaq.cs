namespace DH.Helpdesk.BusinessData.Models.Faq.Input
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ExistingFaq
    {
        #region Constructors and Destructors

        public ExistingFaq(
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
            DateTime changedDate,
            int languageId)
        {
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
            this.LanguageId = languageId;
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string Answer { get; private set; }

        public DateTime ChangedDate { get; private set; }

        [IsId]
        public int FaqCategoryId { get; private set; }

        [IsId]
        public int Id { get; private set; }

        public bool InformationIsAvailableForNotifiers { get; private set; }

        public string InternalAnswer { get; private set; }

        [NotNullAndEmpty]
        public string Question { get; private set; }

        public bool ShowOnStartPage { get; private set; }

        public string UrlOne { get; private set; }

        public string UrlTwo { get; private set; }

        [IsId]
        public int? WorkingGroupId { get; private set; }

        public int LanguageId { get; set; }

        #endregion
    }
}