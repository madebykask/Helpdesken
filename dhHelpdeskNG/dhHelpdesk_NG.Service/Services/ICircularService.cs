namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;

    public interface ICircularService
    {
        List<CircularOverview> GetCircularOverviewsByQuestionnaireId(int questionnaireId);

        void AddCircular(CircularForInsert businessModel);

        void UpdateCircular(CircularForUpdate businessModel);

        CircularForEdit GetById(int id);

        void DeleteById(int id);

        List<CircularPart> GetCases(
            int customerId,
            int questionnaireId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo);
    }
}
