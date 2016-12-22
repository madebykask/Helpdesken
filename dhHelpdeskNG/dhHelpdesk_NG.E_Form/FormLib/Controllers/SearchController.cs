using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using DH.Helpdesk.EForm.FormLib.Models;

namespace DH.Helpdesk.EForm.FormLib.Controllers
{
    public class SearchController : FormLibBaseController
    {
        private readonly IGlobalViewRepository _globalViewRepository;
        private readonly IContractRepository _contractRepository;

        public SearchController(IGlobalViewRepository globalViewRepository
            , IUserRepository userRepository, IContractRepository contractRepository)
            : base(userRepository)
        {
            _globalViewRepository = globalViewRepository;
            _contractRepository = contractRepository;
        }

        [HttpPost]
        public ActionResult GlobalView(string query, int customerId, string searchKey, string formFieldName, bool allCoWorkers = false)
        {
            var result = new object { };

            if (!FormLibUtils.IsSelfService())
                result = _globalViewRepository.GlobalViewSearch(query, customerId, FormLibSessions.User.Id, allCoWorkers, searchKey, formFieldName);
            else
            {
                if (FormLibSessions.CurrentCoWorkers.Any())
                {
                    var employeeNumbers = String.Join("|", FormLibSessions.CurrentCoWorkers.Select(x => x.EmployeeNumber).ToList());

                    if (employeeNumbers == null)
                        employeeNumbers = "";

                    /*
                    if(allCoWorkers)
                        employeeNumbers = null;
                     * */

                    result = _globalViewRepository.GlobalViewSearch(query, customerId, employeeNumbers);
                }
            }

            return Json(result);
        }


        [HttpPost]
        public ActionResult EmployeesExtendedInfo(Guid formGuid, string employeenumber)
        {
            if (string.IsNullOrEmpty(employeenumber))
                return Json(new object { });

            var result = _globalViewRepository.GetEmployeeExtendedInfo(formGuid, employeenumber).ToList();

            //ONLY NO - Change Terms & Conditions //TAN
            //This is because we have different names for Company/Business Unit in DH Helpdesk vs Access Management.
            //This should be solved with the introduction of Data Admin B, when we start using codes instead of matching against strings.
            Guid formGuidCompareCTC = new Guid("d46a39b4-0e58-4b86-9f87-417a9253c18e");


            //ONLY NO - Incorrected Worked Hours //TAN
            //This is because we have different names for Company/Business Unit in DH Helpdesk vs Access Management.
            //This should be solved with the introduction of Data Admin B, when we start using codes instead of matching against strings.
            Guid formGuidCompareIncorrect = new Guid("728B227B-E603-4C6E-A1F3-B46D0F0A5D37");


            if (formGuid == formGuidCompareCTC)
            {
                foreach (var item in result)
                {
                    if (item.FormFieldCode != "")
                    {
                        if (item.FormFieldName == "NewCompany")
                        {
                            item.FormFieldValue = _contractRepository.GetCompanyByCode(item.CustomerId, item.FormFieldCode).Name;
                        }

                        if (item.FormFieldName == "NewBusinessUnit")
                        {
                            item.FormFieldValue = _contractRepository.GetDepartmentByCode(item.CustomerId, item.FormFieldCode).Name;
                        }
                    }

                }

                IEnumerable<GlobalViewExtendedInfo> result2 = result;

                return Json(result2);
            }
            else if (formGuid == formGuidCompareIncorrect)
            {
                foreach (var item in result)
                {
                    if (item.FormFieldCode != "")
                    {
                        if (item.FormFieldName.Contains("WorkedCompany"))
                        {
                            item.FormFieldId = _contractRepository.GetCompanyByCode(item.CustomerId, item.FormFieldCode).Id;
                            item.FormFieldValue = _contractRepository.GetCompanyByCode(item.CustomerId, item.FormFieldCode).Name;
                        }
                    }
               }

                IEnumerable<GlobalViewExtendedInfo> result2 = result;

                return Json(result2);
            }
            else
            {
                return Json(result);
            }
        }
    }
}
