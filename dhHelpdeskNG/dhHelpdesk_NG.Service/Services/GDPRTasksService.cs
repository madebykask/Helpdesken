using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain.GDPR;
using log4net;

namespace DH.Helpdesk.Services.Services
{
    public interface IGDPRTasksService
    {
        GDPRTask GetById(int taskId);
        IList<GDPRTask> GetPendingTasks();
        IList<GDPRTask> GetPendingTasksByFavorite(int favoriteId);


        int AddNewTask(GDPRTask task);
        void UpdateTask(GDPRTask task);

        void UpdateTaskStatus(int taskId, GDPRTaskStatus status);
        GDPRTask GetLatestTask();
    }

    public class GDPRTasksService : IGDPRTasksService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(GDPRTasksService));
        private readonly IGDPRTaskRepository _taskRepository;

        #region ctor()

        public GDPRTasksService(IGDPRTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            //_log.DebugFormat("GDPRTasksService.ctor() called. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        #endregion

        public GDPRTask GetById(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            return task;
        }

        public GDPRTask GetLatestTask()
        {
            var task = _taskRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            return task;
        }

        public IList<GDPRTask> GetPendingTasksByFavorite(int favoriteId)
        {
            var tasks = _taskRepository.GetMany(t => t.FavoriteId == favoriteId && t.Status != GDPRTaskStatus.Complete).OrderBy(x => x.AddedDate).ToList();
            return tasks;
        }

        public IList<GDPRTask> GetPendingTasks()
        {
            var tasks = _taskRepository.GetTasks(GDPRTaskStatus.None).OrderBy(x => x.AddedDate).ToList();
            return tasks;
        }

        public void UpdateTaskStatus(int taskId, GDPRTaskStatus status)
        {
            var task = _taskRepository.GetById(taskId);
            task.Status = status;
            if(status == GDPRTaskStatus.Scheduled)
                task.StartedAt = DateTime.UtcNow;
            _taskRepository.Update(task);
            _taskRepository.Commit();
        }

        public int AddNewTask(GDPRTask task)
        {
            _taskRepository.Add(task);
            _taskRepository.Commit();

            return task.Id;
        }

        public void UpdateTask(GDPRTask task)
        {
            _taskRepository.Update(task);
            _taskRepository.Commit();
        }
    }
}