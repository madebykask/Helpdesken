namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface ICircularRepository : INewRepository
    {
        #region Public Methods and Operators

        List<CircularOverview> FindCircularOverviews(int questionnaireId);

        #endregion
    }
}