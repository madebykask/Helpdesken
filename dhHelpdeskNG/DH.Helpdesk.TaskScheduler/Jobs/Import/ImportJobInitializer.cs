using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace DH.Helpdesk.TaskScheduler.Jobs.Import
{
    internal class ImportJobInitializer : IJobInitializer
    {
        private readonly IScheduler _sched;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IImportInitiatorService _importService;

        public ImportJobInitializer()
        {
        }

        public ImportJobInitializer(IScheduler sched,
                                    IApplicationSettings applicationSettings,
                                    IImportInitiatorService importInitiatorService)
        {
            _sched = sched;
            _applicationSettings = applicationSettings;
            _importService = importInitiatorService;
        }

        public void Run()
        {
            if (!_applicationSettings.IsInitiatorImportEnabled)
                return;

            var _logs = new DataLogModel();
            var initiatorTriggers = _importService.GetTriggers(ref _logs).ToList();


            var i = 0;
            var firstJob = JobBuilder.Create<ImportInitiatorJob>().Build();
            ITrigger firstTrigger = null;
            JobKey lastJobKey = null;
            var listener = new JobChainingJobListener("Pipeline Chain");
            var jobs = new List<IJobDetail>();

            foreach (var initTrigger in initiatorTriggers)
            {
                var initiatorJob =
                    JobBuilder.Create<ImportInitiatorJob>()
                        .WithIdentity($"{Constants.ImportInitiator_JobName}_{i}")
                        .Build();

                if (i == 0)
                {
                    firstJob = initiatorJob;
                    firstTrigger = initTrigger;
                }
                else
                {
                    jobs.Add(initiatorJob);
                    listener.AddJobChainLink(lastJobKey, initiatorJob.Key);
                }

                lastJobKey = initiatorJob.Key;
                i++;
            }

            _sched.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals("Pipeline"));

            _sched.ScheduleJob(firstJob, firstTrigger);
            foreach (var _job in jobs)
            {
                _sched.AddJob(_job, false, true);
            }
        }
    }
}