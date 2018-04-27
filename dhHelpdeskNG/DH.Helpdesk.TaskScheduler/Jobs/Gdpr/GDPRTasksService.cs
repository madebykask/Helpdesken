using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    public interface IGDPRTasksService
    {
        IList<GDPRTask> GetGDPRTaskToRun();
        void UpdateTaskStatus(int taskId, GDPROperationStatus status);
    }

    public class GDPRTasksService : IGDPRTasksService
    {
        private readonly IList<GDPRTask> _tasks  = new List<GDPRTask>();

        public GDPRTasksService()
        {
            //todo: read from the database
            _tasks.Add(new GDPRTask()
            {
                Id = 1, 
                Status = GDPROperationStatus.None,
                FavoriteId = 12,
                AddedDate = DateTime.Now
            });
        }

        public IList<GDPRTask> GetGDPRTaskToRun()
        {
            var tasks = _tasks.Where(t => t.Status == GDPROperationStatus.None).OrderBy(t => t.AddedDate).Select(t => t).ToList();
            return tasks;
        }

        public void UpdateTaskStatus(int taskId, GDPROperationStatus status)
        {
            var task = _tasks.Single(t => t.Id == taskId);
            task.Status = status;
        }
    }
}