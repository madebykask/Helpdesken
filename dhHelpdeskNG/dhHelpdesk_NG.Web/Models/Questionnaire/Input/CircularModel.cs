using System.Linq;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.DisplayValues.Questionnaire;
using DH.Helpdesk.Web.Models.Shared;

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
            IList<int> selectedDepartments,
                                IList<SelectListItem> availableCaseTypes,
                                IList<int> selectedCaseTypes,
                                IList<SelectListItem> availableProductArea,
                                IList<int> selectedProductAreas,
                                IList<SelectListItem> availableWorkingGroups,
                                IList<int> selectedWorkingGroups,
            bool isUniqueEmail,
            string circularName,
            DateTime? changedDate,
            CircularStates circularState,
            List<ConnectedToCircularOverview> connectedCases,
            string extraEmails,
            List<SelectListItem> mailTemplates,
            SendToDialogModel extraEmailsModel)
        {
            CaseFilter = new CircularCaseFilter();
            Id = id;
            this.QuestionnaireId = questionnaireId;
            this.AvailableDepartments = availableDepartments;
            CaseFilter.SelectedDepartments = selectedDepartments;
            this.AvailableCaseTypes = availableCaseTypes;
            CaseFilter.SelectedCaseTypes = selectedCaseTypes;
            this.AvailableProductArea = availableProductArea;
            CaseFilter.SelectedProductAreas = selectedProductAreas;
            this.AvailableWorkingGroups = availableWorkingGroups;
            CaseFilter.SelectedWorkingGroups = selectedWorkingGroups;
            CaseFilter.IsUniqueEmail = isUniqueEmail;
            CircularName = circularName;
            ChangedDate = changedDate;
            State = (CircularStatesDisplayValue) circularState;
            ConnectedCases = connectedCases;
            ExtraEmails = extraEmails;
            MailTemplates = mailTemplates;
            ExtraEmailsModel = extraEmailsModel;
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

        public IList<SelectListItem> AvailableDepartments { get; set; }

        public IList<SelectListItem> AvailableCaseTypes { get; set; }

        public IList<SelectListItem> AvailableProductArea { get; set; }

        public IList<SelectListItem> AvailableWorkingGroups { get; set; }

        public IList<SelectListItem> Procent { get; set; }

        [NotNull]
        public List<ConnectedToCircularOverview> ConnectedCases { get; set; }

        public CircularCaseFilter CaseFilter { get; set; }

        public string ExtraEmails { get; set; }

        public IList<SelectListItem> MailTemplates { get; set; }

        public int? MailTemplateId { get; set; }

        public SendToDialogModel ExtraEmailsModel { get; set; }
    }
}