using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.DisplayValues.Questionnaire;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class CircularModel
    {
        public CircularModel()
        {
        }

        public CircularModel(
            int id,
            int questionnaireId,
            IList<SelectListItem> availableDepartments,
            IList<SelectListItem> selectedDepartments,
                                IList<SelectListItem> availableCaseTypes,
                                IList<SelectListItem> selectedCaseTypes,
                                IList<SelectListItem> availableProductArea,
                                IList<SelectListItem> selectedProductArea,
                                IList<SelectListItem> availableWorkingGroups,
                                IList<SelectListItem> selectedWorkingGroups,
            bool isUniqueEmail,
            string circularName,
            DateTime? changedDate,
            CircularStates circularState,
            List<ConnectedToCircularOverview> connectedCases)
        {
            Id = id;
            this.QuestionnaireId = questionnaireId;
            this.AvailableDepartments = availableDepartments;
            this.SelectedDepartments = selectedDepartments;
            this.AvailableCaseTypes = availableCaseTypes;
            this.SelectedCaseTypes = selectedCaseTypes;
            this.AvailableProductArea = availableProductArea;
            this.SelectedProductArea = selectedProductArea;
            this.AvailableWorkingGroups = availableWorkingGroups;
            this.SelectedWorkingGroups = selectedWorkingGroups;
            this.IsUniqueEmail = isUniqueEmail;
            CircularName = circularName;
            ChangedDate = changedDate;
            State = (CircularStatesDisplayValue) circularState;
            ConnectedCases = connectedCases;
        }

        //[IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("CircularName")]
        public string CircularName { get; set; }

        [LocalizedDisplay("Status")]
        public string Status { get; set; }

        [LocalizedDisplay("State")]
        public CircularStatesDisplayValue State { get; set; }

        [LocalizedDisplay("ModelMode")]
        public int ModelMode { get; set; }

        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        [LocalizedDisplay("ChangedDate")]
        public DateTime? ChangedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishingDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishingDateTo { get; set; }

        public bool IsUniqueEmail { get; set; }

        public IList<SelectListItem> AvailableDepartments { get; set; }

        public IList<SelectListItem> SelectedDepartments { get; set; }

        public IList<SelectListItem> AvailableCaseTypes { get; set; }

        public IList<SelectListItem> SelectedCaseTypes { get; set; }

        public IList<SelectListItem> AvailableProductArea { get; set; }

        public IList<SelectListItem> SelectedProductArea { get; set; }

        public IList<SelectListItem> AvailableWorkingGroups { get; set; }

        public IList<SelectListItem> SelectedWorkingGroups { get; set; }

        public IList<SelectListItem> Procent { get; set; }

        [NotNull]
        public List<ConnectedToCircularOverview> ConnectedCases { get; set; }
    }
}