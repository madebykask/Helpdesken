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

        #region Public Methods and Operators

   
        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            var circulars =
                this.DbContext.QuestionnaireCirculars.Where(q => q.Questionnaire_Id == questionnaireId )
                    .Select(
                        q => new { Id = q.Id, circularName = q.CircularName, date = q.ChangedDate, state = q.Status})
                    .ToList();

            return circulars.Select(q => new CircularOverview(q.Id, q.circularName, q.date, q.state)).ToList();
        }   

        #endregion

    }
}