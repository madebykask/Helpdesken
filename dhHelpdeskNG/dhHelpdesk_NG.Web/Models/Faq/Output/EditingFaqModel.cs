namespace dhHelpdesk_NG.Web.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class EditingFaqModel
    {
        #region Constructors and Destructors

        public EditingFaqModel(
            string id,
            DropDownWithSubmenusContent category,
            string question,
            string answer,
            string internalAnswer,
            List<string> files,
            string urlOne,
            string urlTwo,
            DropDownContent workingGroup,
            bool informationIsAvailableForNotifiers,
            bool showOnStartPage)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category", "Value cannot be null.");
            }

            if (files == null)
            {
                throw new ArgumentNullException("files", "Value cannot be null.");
            }

            if (workingGroup == null)
            {
                throw new ArgumentNullException("workingGroup", "Value cannot be null.");
            }

            this.Id = id;
            this.Category = category;
            this.Question = question;
            this.Answer = answer;
            this.InternalAnswer = internalAnswer;
            this.Files = files;
            this.UrlOne = urlOne;
            this.UrlTwo = urlTwo;
            this.WorkingGroup = workingGroup;
            this.InformationIsAvailableForNotifiers = informationIsAvailableForNotifiers;
            this.ShowOnStartPage = showOnStartPage;
        }

        #endregion

        #region Public Properties

        public string Id { get; private set; }

        public string Answer { get; private set; }

        public DropDownWithSubmenusContent Category { get; private set; }

        public List<string> Files { get; private set; }

        public bool InformationIsAvailableForNotifiers { get; private set; }

        public string InternalAnswer { get; private set; }

        public string Question { get; private set; }

        public bool ShowOnStartPage { get; private set; }

        public string UrlOne { get; private set; }

        public string UrlTwo { get; private set; }

        public DropDownContent WorkingGroup { get; private set; }

        #endregion
    }
}