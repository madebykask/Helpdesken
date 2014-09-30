namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;

    public sealed class CircularRepository : Repository, ICircularRepository
    {
        #region Constructors and Destructors

        public CircularRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        public void AddCircular(NewCircular newCircular)
        {
            var circularEntity = new QuestionnaireCircularEntity()
            {
                Questionnaire_Id = newCircular.QuestionnaireId,
                CircularName = newCircular.CircularName,
                Status = newCircular.Status,
                ChangedDate = newCircular.ChangedDate,
                CreatedDate = newCircular.ChangedDate
            };

            this.DbContext.QuestionnaireCirculars.Add(circularEntity);
            this.InitializeAfterCommit(newCircular, circularEntity);
        }

        public void UpdateCircular(EditCircular editedCircular)
        {
            var circularEntity = this.DbContext.QuestionnaireCirculars.Find(editedCircular.Id);

            circularEntity.CircularName = editedCircular.CircularName;
            circularEntity.ChangedDate = editedCircular.ChangedDate;            
        }
    }
}