namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
    using DH.Helpdesk.Services.Response.Questionnaire;

    public interface ICircularService
    {
        List<CircularOverview> GetCircularOverviews(int questionnaireId, int state);

        void AddCircular(CircularForInsert businessModel);

        void UpdateCircular(CircularForUpdate businessModel);

        CircularForEdit GetById(int id);

	    CircularForEdit GetSingleOrDefaultByQuestionnaireId(int id);

		void DeleteById(int id);

        void DeleteConnectedCase(int cirularId, int caseId);
	    List<int> GetAllCircularCasesIds(int circularId);

		QuestionnaireDetailedOverview GetQuestionnaire(Guid guid, OperationContext operationContext);

        QuestionnaireOverview GetQuestionnaire(int id, OperationContext operationContext);

        List<OptionResult> GetResult(int circularId);

        List<OptionResult> GetResults(List<int> circularIds, DateTime? from, DateTime? to);

        List<AvailableCase> GetAvailableCases(
            int customerId,
            int questionnaireId,
            IList<int> selectedDepartments,
            IList<int> selectedCaseTypes,
            IList<int> selectedProductArea,
            IList<int> selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo,
            bool isUniqueEmail);

        List<ConnectedCase> GetConnectedCases(int id);

        void SendQuestionnaire(string actionAbsolutePath, int circularId, OperationContext operationContext);

        void Remind(string actionAbsolutePath, int circularId, OperationContext operationContext);

        void SaveAnswers(ParticipantForInsert businessModel);

	    List<BusinessLogic.MapperData.Participant> GetNotAnsweredParticipants(int circularId);

    }
}
