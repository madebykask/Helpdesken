using System;
using System.Web.Mvc;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;

    public interface ICircularService
    {
        List<CircularOverview> FindCircularOverviews(int questionnaireId);

        void AddCircular(NewCircular newCircular);

        void UpdateCircular(EditCircular editedCircular);

        EditCircular GetCircularById(int circularId);

        void DeleteCircularById(int deletedCircularId);

        List<CircularPart> GetCases(int customerId,                                                              
                                    int[] selectedDepartments,                                     
                                    int[] selectedCaseTypes,                                    
                                    int[] selectedProductArea,                                    
                                    int[] selectedWorkingGroups,
                                    int procent,
                                    DateTime? finishingDateFrom,
                                    DateTime? finishingDateTo
                                   );
    }
}
