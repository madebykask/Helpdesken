
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.BusinessData.Models.DailyReport.Input;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.DailyReports;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface IDailyReportService
    {
        IList<DailyReportOverview> GetDailyReports(int customerId);

        DailyReportOverview GetDailyReport(int customerId , int id);

        IList<DailyReportSubject> GetDailyReportSubjects(int customerId);

        IList<DailyReportSubjectBM> GetAllDailyReportSubjects(int customerId);

        DailyReportSubject GetDailyReportSubject(int id);

        DeleteMessage DeleteDailyReportSubject(int id);

        DeleteMessage DeleteDailyReport(int id);   

        void SaveDailyReportSubject(DailyReportSubject dailyReportSubject, out IDictionary<string, string> errors);
        void Commit();
        IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers, int? count);

        void SaveDailyReport(DailyReportUpdate updatedDailyReport);
    }

    public class DailyReportService : IDailyReportService
    {
        private readonly IDailyReportSubjectRepository _dailyReportSubjectRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IDailyReportRepository _dailyReportRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

#pragma warning disable 0618
        public DailyReportService(
            IDailyReportSubjectRepository dailyReportSubjectRepository,
            IUnitOfWork unitOfWork,
            IDailyReportRepository dailyReportRepository, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._dailyReportSubjectRepository = dailyReportSubjectRepository;
            this._unitOfWork = unitOfWork;
            _dailyReportRepository = dailyReportRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
#pragma warning restore 0618

        public IList<DailyReportSubject> GetDailyReportSubjects(int customerId)
        {
            return this._dailyReportSubjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Subject).ToList();
        }

        public IList<DailyReportSubjectBM> GetAllDailyReportSubjects(int customerId)
        {
            var dailyReportSubjectEntity  = 
                  this._dailyReportSubjectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Subject).ToList();
            
            if (dailyReportSubjectEntity != null)
            {
                var dailyReportSubject = dailyReportSubjectEntity.Select(d=> new 
                    DailyReportSubjectBM(d.Customer_Id, d.Id, d.IsActive, d.ShowOnStartPage, d.Subject, d.ChangedDate, d.CreatedDate)).ToList();
                return dailyReportSubject;
            }
            else
              return null;
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

        public DeleteMessage DeleteDailyReport(int id)
        {
            var dailyReport = this._dailyReportRepository.GetById(id);

            if (dailyReport != null)
            {
                try
                {
                    this._dailyReportRepository.Delete(dailyReport);
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

        public void SaveDailyReport(DailyReportUpdate dailyReport)
        {

            if (dailyReport == null)
                throw new ArgumentNullException("dailyreport");

            var dailyReportEntity = new DailyReport()
            {
                Customer_Id = dailyReport.CustomerId,
                Id = dailyReport.Id,
                MailSent = dailyReport.Sent,
                User_Id = dailyReport.UserId,
                DailyReportText = dailyReport.DailyReportText,
                DailyReportSubject_Id = dailyReport.DailyReportSubjectId,
                CreatedDate = dailyReport.ChangedDate,
                ChangedDate = dailyReport.ChangedDate
            };

            if (dailyReport.Id == 0)
                this._dailyReportRepository.Add(dailyReportEntity);
            else
                this._dailyReportRepository.Update(dailyReportEntity);
            
            this.Commit();
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

        /// <summary>
        /// The get DailyReports.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<DailyReportOverview> GetDailyReports(int customerId)
        {
            return this.GetDailyReportOverviews(new[] { customerId }, null).ToList();
        }

        public DailyReportOverview GetDailyReport(int customerId ,int id )
        {
            return this.GetDailyReports(customerId).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DailyReportOverview> GetDailyReportOverviews(int[] customers, int? count)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<DailyReport>();

                return repository.GetAll()
                        .GetForStartPage(customers, count)
                        .MapToOverviews();
            }
        }
    }
}
