using System;
using System.Web.Mvc;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using System.Globalization;

    public class CircularService : ICircularService
    {
        #region Fields        

        private readonly IQuestionnaireRepository _questionnaireRepository;

        private readonly ICircularRepository _circularRepository;

        private readonly ICaseRepository _caseRepository;

        private readonly IUserService _userService;

        #endregion

        #region Constructors and Destructors

        public CircularService(
            IQuestionnaireRepository questionnaireRepository,
            ICircularRepository circularRepository,
            ICaseRepository caseRepository,
            IUserService userService
            )
        {
            this._questionnaireRepository = questionnaireRepository;
            this._circularRepository = circularRepository;
            this._caseRepository = caseRepository;
            this._userService = userService;
        }

        #endregion

        #region Public Methods and Operators


        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            return this._circularRepository.FindCircularOverviews(questionnaireId);
        }

        public void AddCircular(NewCircular newCircular)
        {
            _circularRepository.AddCircular(newCircular);
            this._circularRepository.Commit();
        }

        public EditCircular GetCircularById(int circularId)
        {
            return _circularRepository.GetCircularById(circularId);
        }

        public void UpdateCircular(EditCircular editedCircular)
        {
            _circularRepository.UpdateCircular(editedCircular);
            this._circularRepository.Commit();
        }

        public void DeleteCircularById(int deletedCircularId)
        {
            _circularRepository.DeleteCircularById(deletedCircularId);
            this._circularRepository.Commit();
        }

        public List<CircularPart> GetCases(int customerId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo
            )

        {

            var cases = _caseRepository.GetAll()
                .Where(
                    c =>
                        c.Customer_Id == customerId && c.Deleted == 0 && c.FinishingDate != null &&
                        c.PersonsEmail.Length > 3)
                .ToList();

            //.Select(c => new { Case_Id = c.Id, CaseNumber = c.CaseNumber, Caption = c.Caption, Email = c.PersonsEmail})                                                  

            if (selectedDepartments != null && selectedDepartments.Count() > 0)
                cases =
                    cases.Where(
                        c => c.Department_Id != null && selectedDepartments.ToList().Contains(c.Department_Id.Value))
                        .ToList();

            if (selectedCaseTypes != null && selectedCaseTypes.Count() > 0)
                cases = cases.Where(c => selectedCaseTypes.ToList().Contains(c.CaseType_Id)).ToList();

            if (selectedProductArea != null && selectedProductArea.Count() > 0)
                cases =
                    cases.Where(
                        c => c.ProductArea_Id != null && selectedProductArea.ToList().Contains(c.ProductArea_Id.Value))
                        .ToList();


            if (selectedWorkingGroups != null && selectedWorkingGroups.Count() > 0)
            {
                var users = _userService.GetUsers(customerId)
                    .Where(
                        u =>
                            u.Default_WorkingGroup_Id != null &&
                            selectedWorkingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value))
                    .Select(u => u.Id)
                    .ToList();

                cases = cases.Where(c => users.ToList().Contains(c.Performer_User_Id)).ToList();
            }

            var ret = new List<CircularPart>();
            foreach (var c in cases)
                ret.Add(new CircularPart(c.Id, (int) c.CaseNumber, c.Caption, c.PersonsEmail));

            return ret;

        }

        #endregion
    }
}