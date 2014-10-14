namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;

    public interface ICircularService
    {
        List<CircularOverview> GetCircularOverviews(int questionnaireId, int state);

        void AddCircular(CircularForInsert businessModel);

        void UpdateCircular(CircularForUpdate businessModel);

        CircularForEdit GetById(int id);

        void DeleteById(int id);

        void DeleteConnectedCase(int cirularId, int caseId);

        List<AvailableCase> GetAvailableCases(
            int customerId,
            int questionnaireId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo,
            bool isUniqueEmail);

        List<ConnectedCase> GetConnectedCases(int id);

        void SendQuestionnaire(string actionAbsolutePath, int circularId, OperationContext operationContext);
    }
}
