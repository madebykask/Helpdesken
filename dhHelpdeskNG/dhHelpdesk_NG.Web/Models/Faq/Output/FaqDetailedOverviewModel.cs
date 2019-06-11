namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    public sealed class FaqDetailedOverviewModel : FaqOverviewModel
    {
        public FaqDetailedOverviewModel(int id, DateTime createdDate, DateTime changedDate, string text, string answer, string internalAnswer, List<string> fileNames, string urlOne, string urlTwo)
            : base(id, createdDate, changedDate, text)
        {
            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentNullException("answer", "Value cannot be null or empty.");
            }

            if (fileNames == null)
            {
                throw new ArgumentNullException("fileNames", "Value cannot be null.");
            }

            this.Answer = answer;
            this.InternalAnswer = internalAnswer;
            this.FileNames = fileNames;
            this.UrlOne = urlOne;
            this.UrlTwo = urlTwo;
        }

        public string Answer { get; private set; }

        public string InternalAnswer { get; private set; }

        public string UrlOne { get; private set; }

        public string UrlTwo { get; private set; }

        public List<string> FileNames { get; private set; }
    }
}