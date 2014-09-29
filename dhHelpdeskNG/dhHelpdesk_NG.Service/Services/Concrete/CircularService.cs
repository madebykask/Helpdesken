namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public class CircularService : ICircularService
    {
        #region Fields

        private readonly ICircularRepository circularRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public CircularService(ICircularRepository circularRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.circularRepository = circularRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators


        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            return this.circularRepository.FindCircularOverviews(questionnaireId);
        }

        public void AddCircular(NewCircular newCircular)
        {
            this.circularRepository.AddCircular(newCircular);
            this.circularRepository.Commit();
        }

        public EditCircular GetCircularById(int circularId)
        {
            return this.circularRepository.GetCircularById(circularId);
        }

        public void UpdateCircular(EditCircular editedCircular)
        {
            this.circularRepository.UpdateCircular(editedCircular);
            this.circularRepository.Commit();
        }

        public void DeleteCircularById(int deletedCircularId)
        {
            this.circularRepository.DeleteCircularById(deletedCircularId);
            this.circularRepository.Commit();
        }

        public List<CircularPart> GetCases(
            int customerId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo)
        {
            const int MinEmailLength = 3;
            const int IsDeleted = 0;

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var caseRepo = uof.GetRepository<Case>();
                var userRepo = uof.GetRepository<User>();

                IQueryable<Case> query =
                    caseRepo.GetAll()
                        .GetUsersByCustomer(customerId)
                        .Where(
                            c =>
                            c.Deleted == IsDeleted && c.FinishingDate != null && c.PersonsEmail.Length > MinEmailLength);

                if (selectedDepartments != null && selectedDepartments.Any())
                {
                    query =
                        query.Where(
                            c => c.Department_Id != null && selectedDepartments.ToList().Contains(c.Department_Id.Value));
                }

                if (selectedCaseTypes != null && selectedCaseTypes.Any())
                {
                    query = query.Where(c => selectedCaseTypes.ToList().Contains(c.CaseType_Id));
                }

                if (selectedProductArea != null && selectedProductArea.Any())
                {
                    query =
                        query.Where(
                            c =>
                            c.ProductArea_Id != null && selectedProductArea.ToList().Contains(c.ProductArea_Id.Value));
                }

                if (selectedWorkingGroups != null && selectedWorkingGroups.Any())
                {
                    IQueryable<int> userIds =
                        userRepo.GetAll()
                            .GetUsersByCustomer(customerId)
                            .Where(
                                u =>
                                u.Default_WorkingGroup_Id != null
                                && selectedWorkingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value))
                            .Select(u => u.Id);

                    query = query.Where(c => userIds.ToList().Contains(c.Performer_User_Id));
                }

                if (finishingDateFrom.HasValue)
                {
                    query = query.Where(x => x.FinishingDate >= finishingDateFrom);
                }

                if (finishingDateTo.HasValue)
                {
                    query = query.Where(x => x.FinishingDate <= finishingDateTo);
                }

                int percentageOfCases = caseRepo.GetAll().Count() / procent;

                query = query.Take(percentageOfCases);

                var anonymus = query.Select(x => new { x.Id, x.CaseNumber, x.Caption, x.PersonsEmail }).ToList();

                List<CircularPart> businessModels =
                    anonymus.Select(c => new CircularPart(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail)).ToList();

                return businessModels;
            }
        }
        #endregion
    }
}