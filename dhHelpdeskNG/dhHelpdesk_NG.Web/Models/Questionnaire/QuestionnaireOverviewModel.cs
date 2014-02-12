namespace DH.Helpdesk.Web.Models.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class QuestionnaireOverviewModel
    {
      
        #region Constructors and Destructors
        
        public QuestionnaireOverviewModel(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
      
        #endregion

        #region Public Properties

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("Name")]
        public string Name { get; set; }

        [LocalizedDisplay("Description")]
        public string Description { get; set; }
        
        #endregion
    }
}