using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class NewCircularModel
    {
        public NewCircularModel(int questionnaireId, 
                                IList<SelectListItem> availableDepartments, 
                                IList<SelectListItem> selectedDepartments, 
                                IList<SelectListItem> availableCaseTypes,
                                IList<SelectListItem> selectedCaseTypes,
                                IList<SelectListItem> availableProductArea,
                                IList<SelectListItem> selectedProductArea,
                                IList<SelectListItem> availableWorkingGroups,
                                IList<SelectListItem> selectedWorkingGroups
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

        }

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [Required]
        [StringLength(100)]
        [LocalizedDisplay("CircularName")]
        public string CircularName { get; set; }
        
        [LocalizedDisplay("Status")]
        public string Status { get; set; }
        
        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        public IList<SelectListItem> AvailableDepartments { get; set; }
        public IList<SelectListItem> SelectedDepartments { get; set; }

        public IList<SelectListItem> AvailableCaseTypes { get; set; }
        public IList<SelectListItem> SelectedCaseTypes { get; set; }

        public IList<SelectListItem> AvailableProductArea { get; set; }
        public IList<SelectListItem> SelectedProductArea { get; set; }

        public IList<SelectListItem> AvailableWorkingGroups { get; set; }
        public IList<SelectListItem> SelectedWorkingGroups { get; set; }

        public IList<SelectListItem> Procent { get; set; }

    }
}