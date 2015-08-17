namespace DH.Helpdesk.Web.Infrastructure.Case
{
    using System;

    using DH.Helpdesk.Web.Models.Case;

    public static class CaseViewMapper
    {
        public static void MapCaseToCaseInputViewModel(this CaseInputViewModel inputModel, Domain.Case case_, TimeZoneInfo userTimeZone)
        {
            inputModel.RegTime = TimeZoneInfo.ConvertTimeFromUtc(case_.RegTime.ToUniversalTime(), userTimeZone);  
        }
    }
}