namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public interface ISurveyService
    {
        Survey GetByCaseId(int caseId);

        void SaveSurvey(Survey item);

       Tuple<int, int, int, int> GetSurveyStat(
            int customerId,
            DateTime finishingDateFrom,
            DateTime finishingDateTo,
            int[] selectedCaseTypes,
            int? selectedProductArea,
            int[] selectedWorkingGroups);
    }
}
