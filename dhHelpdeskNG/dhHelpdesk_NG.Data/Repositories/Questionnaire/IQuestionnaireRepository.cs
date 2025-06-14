﻿using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IQuestionnaireRepository : INewRepository
    {
        #region Public Methods and Operators

        void AddSwedishQuestionnaire(NewQuestionnaire questionnaire);

        void DeleteQuestionnaireById(int questionnaireId);

        List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId);
		List<QuestionnaireEntity> FindFeedbackOverviews(int customerId);

		EditQuestionnaire GetQuestionnaireById(int id, int languageId);

        void UpdateOtherLanguageQuestionnaire(EditQuestionnaire questionnaire);

        void UpdateSwedishQuestionnaire(EditQuestionnaire questionnaire);
		List<QuestionnaireEntity> GetFeedbackFullItems(int customerId, IEnumerable<string> identifiers = null);

		#endregion
	}
}