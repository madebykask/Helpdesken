namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using System.Globalization;

    public class CircularService : ICircularService
    {
        #region Fields        

        private readonly IQuestionnaireRepository _questionnaireRepository;

        private readonly ICircularRepository _circularRepository;        

        #endregion

        #region Constructors and Destructors

        public CircularService(
            IQuestionnaireRepository questionnaireRepository,
            ICircularRepository circularRepository
            )
        {
            this._questionnaireRepository = questionnaireRepository;            
            this._circularRepository = circularRepository;
        }

        #endregion

        #region Public Methods and Operators

      
        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            return this._circularRepository.FindCircularOverviews(questionnaireId);
        }      

        #endregion
    }
}