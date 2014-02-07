namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IDailyReportService
    {
        IList<DailyReportSubject> GetDailyReportSubjects(int customerId);

        DailyReportSubject GetDailyReportSubject(int id);

        DeleteMessage DeleteDailyReportSubject(int id);

        void SaveDailyReportSubject(DailyReportSubject dailyReportSubject, out IDictionary<string, string> errors);
        void Commit();
    }

    public class DailyReportService : IDailyReportService
    {
        private readonly IDailyReportSubjectRepository _dailyReportSubjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DailyReportService(
            IDailyReportSubjectRepository dailyReportSubjectRepository,
            IUnitOfWork unitOfWork)
        {
            this._dailyReportSubjectRepository = dailyReportSubjectRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<DailyReportSubject> GetDailyReportSubjects(int customerId)
        {
            return this._dailyReportSubjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Subject).ToList();
        }

        public DailyReportSubject GetDailyReportSubject(int id)
        {
            return this._dailyReportSubjectRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteDailyReportSubject(int id)
        {
            var dailyReportSubject = this._dailyReportSubjectRepository.GetById(id);

            if (dailyReportSubject != null)
            {
                try
                {
                    this._dailyReportSubjectRepository.Delete(dailyReportSubject);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveDailyReportSubject(DailyReportSubject dailyReportSubject, out IDictionary<string, string> errors)
        {
            if (dailyReportSubject == null)
                throw new ArgumentNullException("dailyreportsubject");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(dailyReportSubject.Subject))
                errors.Add("DailyReportSubject.Subject", "Du måste ange ett delområde");

            dailyReportSubject.ChangedDate = DateTime.UtcNow;

            if (dailyReportSubject.Id == 0)
                this._dailyReportSubjectRepository.Add(dailyReportSubject);
            else
                this._dailyReportSubjectRepository.Update(dailyReportSubject);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
