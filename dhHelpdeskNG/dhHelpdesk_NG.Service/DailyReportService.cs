using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
            _dailyReportSubjectRepository = dailyReportSubjectRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<DailyReportSubject> GetDailyReportSubjects(int customerId)
        {
            return _dailyReportSubjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Subject).ToList();
        }

        public DailyReportSubject GetDailyReportSubject(int id)
        {
            return _dailyReportSubjectRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteDailyReportSubject(int id)
        {
            var dailyReportSubject = _dailyReportSubjectRepository.GetById(id);

            if (dailyReportSubject != null)
            {
                try
                {
                    _dailyReportSubjectRepository.Delete(dailyReportSubject);
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
                _dailyReportSubjectRepository.Add(dailyReportSubject);
            else
                _dailyReportSubjectRepository.Update(dailyReportSubject);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
