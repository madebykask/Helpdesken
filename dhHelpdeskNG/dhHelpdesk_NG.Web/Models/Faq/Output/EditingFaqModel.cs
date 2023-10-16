using System.Web.Mvc;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
            bool showOnStartPage,
            SelectList languages,
            int languageId,
            bool showDetails = false)
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
            this.Languages = languages;
            this.LanguageId = languageId;

            var ff = new FAQFileModel() { FAQId = id, FAQFiles = files, LanguageId = languageId};
            this.FAQLFile = ff;
            this.ShowDetails = showDetails;
        }

        #endregion

        #region Public Properties

        public string Id { get; private set; }

        [LocalizedRequired("Du måste ange ett svar")]
        [LocalizedStringLength(3000)]
        public string Answer { get; private set; }

        public DropDownWithSubmenusContent Category { get; private set; }

        public List<string> Files { get; private set; }

        public FAQFileModel FAQLFile { get; private set; }

        public bool InformationIsAvailableForNotifiers { get; private set; }

        [LocalizedStringLength(3000)]
        public string InternalAnswer { get; private set; }

        [LocalizedRequired]
        public string Question { get; private set; }

        public bool ShowOnStartPage { get; private set; }

        [LocalizedStringLength(2000)]
        public string UrlOne { get; private set; }

        [LocalizedStringLength(2000)]
        public string UrlTwo { get; private set; }

        public DropDownContent WorkingGroup { get; private set; }

        [LocalizedDisplay("LanquageId")]
        public int LanguageId { get; set; }

        public SelectList Languages { get; set; }

        public bool ShowDetails { get; set; }

        public bool IsNew
        {
            get
            {
                int result;
                return !int.TryParse(Id, out result);
            }
        }

        #endregion
    }
}