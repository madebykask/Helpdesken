using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web.DynamicData;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.LocalizedAttributes;

    public class CircularPartOverview
    {
        public CircularPartOverview()
        {
            
        }

        public CircularPartOverview(int caseId, int caseNumber, string caption, string email)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
        }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }
    } 

    public class NewCircularModel
    {
        public NewCircularModel()
        {
            
        }

        public NewCircularModel(int questionnaireId, 
                                IList<SelectListItem> availableDepartments, 
                                IList<SelectListItem> selectedDepartments, 
                                IList<SelectListItem> availableCaseTypes,
                                IList<SelectListItem> selectedCaseTypes,
                                IList<SelectListItem> availableProductArea,
                                IList<SelectListItem> selectedProductArea,
                                IList<SelectListItem> availableWorkingGroups,
                                IList<SelectListItem> selectedWorkingGroups,
                                List<CircularPartOverview> circularParts 
            )
        {
            this.QuestionnaireId = questionnaireId;
            this.AvailableDepartments = availableDepartments;
            this.SelectedDepartments = selectedDepartments;
            this.AvailableCaseTypes = availableCaseTypes;
            this.SelectedCaseTypes = selectedCaseTypes;
            this.AvailableProductArea = availableProductArea;
            this.SelectedProductArea = selectedProductArea;
            this.AvailableWorkingGroups = availableWorkingGroups;
            this.SelectedWorkingGroups = selectedWorkingGroups;
            this.CircularParts = circularParts;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }
        
        [StringLength(100)]
        [LocalizedDisplay("CircularName")]
        public string CircularName { get; set; }
        
        [LocalizedDisplay("Status")]
        public string Status { get; set; }

        [LocalizedDisplay("ModelMode")]
        public int ModelMode { get; set; }

        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishingDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishingDateTo { get; set; }


        public IList<SelectListItem> AvailableDepartments { get; set; }
        public IList<SelectListItem> SelectedDepartments { get; set; }

        public IList<SelectListItem> AvailableCaseTypes { get; set; }
        public IList<SelectListItem> SelectedCaseTypes { get; set; }

        public IList<SelectListItem> AvailableProductArea { get; set; }
        public IList<SelectListItem> SelectedProductArea { get; set; }

        public IList<SelectListItem> AvailableWorkingGroups { get; set; }
        public IList<SelectListItem> SelectedWorkingGroups { get; set; }

        public IList<SelectListItem> Procent { get; set; }

        public List<CircularPartOverview> CircularParts { get; set; }

    }
}