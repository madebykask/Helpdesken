using DH.Helpdesk.Common.Enums;

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

        int AddCircular(CircularForInsert businessModel);

        void UpdateCircular(CircularForUpdate businessModel);

        CircularForEdit GetById(int id);

	    CircularForEdit GetSingleOrDefaultByQuestionnaireId(int id);

		void DeleteById(int id);

        void DeleteConnectedCase(int cirularId, int caseId);
	    List<int> GetAllCircularCasesIds(int circularId);

        QuestionnaireDetailedOverview GetQuestionnaire(Guid guid, int languageId);
        QuestionnaireOverview GetQuestionnaire(int id, OperationContext operationContext);

        List<OptionResult> GetResult(int circularId);

        List<OptionResult> GetResults(List<int> circularIds, DateTime? from, DateTime? to);

        List<OptionResult> GetResults(int circularId, DateTime? from, DateTime? to, List<int> departmentIds);

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

        int SaveAnswers(ParticipantForInsert businessModel);

	    List<BusinessLogic.MapperData.Participant> GetNotAnsweredParticipants(int circularId);

        void SetStatus(int circularId, CircularStates circularState);
        void UpdateParticipantSendDate(Guid participantGuid, DateTime operationDate);
        int GetCircularIdByQuestionnaireId(int questionaireId);
        void SaveFeedbackNote(int questionId, string noteText);
        List<string> GetCircularExtraEmails(int circularId);
    }
}
