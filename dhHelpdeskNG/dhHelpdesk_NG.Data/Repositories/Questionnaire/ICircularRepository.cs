namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.Dal.Dal;

    public interface ICircularRepository : INewRepository
    {
        #region Public Methods and Operators

        void AddCircular(NewCircular newCircular);

        void UpdateCircular(EditCircular editedCircular);

        #endregion
    }
}