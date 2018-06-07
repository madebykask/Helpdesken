using DH.Helpdesk.Dal.Repositories.GDPR;

namespace DH.Helpdesk.Services.BusinessLogic.Gdpr
{
    public interface IDataPrivacyTaskProgress
    {
        void Update(int taskId, int progress);
    }

    public class DataPrivacyTaskProgress : IDataPrivacyTaskProgress
    {
        private readonly IGDPRTaskRepository _gdprTaskRepository;

        public DataPrivacyTaskProgress(IGDPRTaskRepository gdprTaskRepository)
        {
            _gdprTaskRepository = gdprTaskRepository;
        }

        public void Update(int taskId, int progress)
        {
            var task = _gdprTaskRepository.GetById(taskId);
            task.Progress = progress;
            _gdprTaskRepository.Update(task);
            _gdprTaskRepository.Commit();
        }
    }
}