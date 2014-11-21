namespace DH.Helpdesk.Mobile.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class EditFaqModel
    {
        #region Constructors and Destructors

        public EditFaqModel(
            int id, 
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
            //if (id <= 0)
            //{
            //    throw new ArgumentOutOfRangeException("id", "Must be more than zero.");
            //}

            //if (category == null)
            //{
            //    throw new ArgumentNullException("category", "Value cannot be null.");
            //}

            //if (string.IsNullOrEmpty(question))
            //{
            //    throw new ArgumentNullException("question", "Value cannot be null or empty.");
            //}

            //if (string.IsNullOrEmpty(answer))
            //{
            //    throw new ArgumentNullException("answer", "Value cannot be null or empty.");
            //}

            //if (files == null)
            //{
            //    throw new ArgumentNullException("files", "Value cannot be null.");
            //}

            //if (workingGroup == null)
            //{
            //    throw new ArgumentNullException("workingGroup", "Value cannot be null.");
            //}

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

        public string Answer { get; private set; }

        public DropDownWithSubmenusContent Category { get; private set; }

        public List<string> Files { get; private set; }

        public int Id { get; private set; }

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