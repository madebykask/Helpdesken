namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;

    public interface ICircularService
    {
        List<CircularOverview> FindCircularOverviews(int questionnaireId);        
    }
}
