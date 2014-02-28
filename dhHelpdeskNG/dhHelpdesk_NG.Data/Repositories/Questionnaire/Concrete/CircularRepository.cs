using System.Security.Cryptography.X509Certificates;

namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Common.Enums;
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

        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            var circulars =
                this.DbContext.QuestionnaireCirculars.Where(q => q.Questionnaire_Id == questionnaireId)
                    .Select(
                        c => new {Id = c.Id, circularName = c.CircularName, date = c.ChangedDate, state = c.Status})
                    .ToList();

            return circulars.Select(q => new CircularOverview(q.Id, q.circularName, q.date, q.state)).ToList();
        }

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

        public EditCircular GetCircularById(int circularId)
        {
            EditCircular ret = null;
            var circular =
                this.DbContext.QuestionnaireCirculars.Where(q => q.Id == circularId)
                    .Select(
                        c => new {Id = c.Id, circularName = c.CircularName, date = c.ChangedDate, state = c.Status})
                    .FirstOrDefault();

            if (circular != null)
                ret = new EditCircular(circular.Id, circular.circularName, circular.state, circular.date);

            return ret;
        }

        public void DeleteCircularById(int deletedCircularId)
        {

            var circular = this.DbContext.QuestionnaireCirculars.Find(deletedCircularId);

            if (circular != null)
                this.DbContext.QuestionnaireCirculars.Remove(circular);
        }


    }
}