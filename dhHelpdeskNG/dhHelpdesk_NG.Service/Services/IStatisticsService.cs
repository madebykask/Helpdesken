﻿namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;

    public interface IStatisticsService
    {
        StatisticsOverview GetStatistics(
                        int[] customers, 
                        int userId);
    }
}