using DH.Helpdesk.Domain.MailTemplates;

namespace DH.Helpdesk.Domain.Questionnaire
{
    using global::System;
    using global::System.Collections.Generic;

    public class QuestionnaireCircularEntity : Entity
    {
        public QuestionnaireCircularEntity()
        {
            this.QuestionnaireCircularPartEntities = new List<QuestionnaireCircularPartEntity>();
            QuestionnaireCircularDepartmentEntities = new List<QuestionnaireCircularDepartmentEntity>();
            QuestionnaireCircularCaseTypeEntities = new List<QuestionnaireCircularCaseTypeEntity>();
            QuestionnaireCircularProductAreaEntities = new List<QuestionnaireCircularProductAreaEntity>();
            QuestionnaireCircularWorkingGroupEntities = new List<QuestionnaireCircularWorkingGroupEntity>();
            QuestionnaireCircularExtraEmailEntities = new List<QuestionnaireCircularExtraEmailEntity>();
        }

        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public string CircularName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Questionnaire_Id { get; set; }

        public int Status { get; set; }

        public bool IsUniqueEmail { get; set; }
        
        public DateTime? FinishingDateFrom { get; set; }

        public DateTime? FinishingDateTo { get; set; }

        public int SelectedProcent { get; set; }

        public int? MailTemplate_Id { get; set; }

        public virtual QuestionnaireEntity Questionnaire { get; set; }

        public virtual ICollection<QuestionnaireCircularPartEntity> QuestionnaireCircularPartEntities { get; set; }

        public virtual ICollection<QuestionnaireCircularDepartmentEntity> QuestionnaireCircularDepartmentEntities { get; set; }

        public virtual ICollection<QuestionnaireCircularCaseTypeEntity> QuestionnaireCircularCaseTypeEntities { get; set; }

        public virtual ICollection<QuestionnaireCircularProductAreaEntity> QuestionnaireCircularProductAreaEntities { get; set; }

        public virtual ICollection<QuestionnaireCircularWorkingGroupEntity> QuestionnaireCircularWorkingGroupEntities { get; set; }

        public virtual ICollection<QuestionnaireCircularExtraEmailEntity> QuestionnaireCircularExtraEmailEntities { get; set; }

        #endregion
    }
}