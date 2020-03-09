using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;

    public interface IStatisticsService
    {
        StatisticsOverview GetStatistics(int[] customers, UserOverview userId);
    }
}