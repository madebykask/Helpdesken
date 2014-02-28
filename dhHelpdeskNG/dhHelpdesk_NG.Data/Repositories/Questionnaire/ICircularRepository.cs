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

        void AddCircular(NewCircular newCircular);

        void UpdateCircular(EditCircular editedCircular);

        EditCircular GetCircularById(int circularId);

        void DeleteCircularById(int deletedCircularId);

        #endregion
    }
}