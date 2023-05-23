using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Shared;
using Microsoft.Reporting.WebForms;
using DH.Helpdesk.Services.Utils;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Controllers
{
    public partial class CasesController
    {
        public JsonResult MarkAsUnread(int id, int customerId)
        {
            this._caseService.MarkAsUnread(id);
            //return this.RedirectToAction("index", "cases", new { customerId = customerId });
            return Json("Success");
        }

        public PartialViewResult ShowCasePrintPreview(int caseId, int caseNumber, bool popupShow = false,
            bool showPrintButton = true)
        {
            var model = GetCaseReportViewerData("CaseDetailsList", caseId, caseNumber);
            model.CanShow = true;
            model.PopupShow = popupShow;
            model.ShowPrintButton = showPrintButton;
            return PartialView("_CasePrint", model);
        }

        public RedirectToRouteResult Copy(int id, int customerId)
        {
            return this.RedirectToAction("new", "cases", new { customerId = customerId, copyFromCaseId = id });
        }

        public RedirectToRouteResult FollowUpRemove(int id)
        {
            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                _caseFollowUpService.RemoveFollowUp(userId, id);
            }

            //            this._caseService.UpdateFollowUpDate(id, null);
            return this.RedirectToAction("edit", "cases", new { id = id, redirectFrom = "save" });
        }

        public RedirectToRouteResult FollowUp(int id)
        {
            if (SessionFacade.CurrentUser != null)
            {
                var userId = SessionFacade.CurrentUser.Id;
                _caseFollowUpService.AddUpdateFollowUp(userId, id);
            }

            //            this._caseService.UpdateFollowUpDate(id, DateTime.UtcNow);
            return this.RedirectToAction("edit", "cases", new { id = id, redirectFrom = "save" });
        }

        public RedirectToRouteResult Activate(int id, string backUrl = null)
        {
            IDictionary<string, string> errors;
            string adUser = global::System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			if (SessionFacade.CurrentUser != null)
				if (SessionFacade.CurrentUser.ActivateCasePermission == 1)
				{
					this._caseService.Activate(id, SessionFacade.CurrentUser.Id, adUser,
						CreatedByApplications.Helpdesk5, out errors);
				}

            return this.RedirectToAction("edit", "cases", new { id, redirectFrom = "save", backUrl });
        }

        #region private
        private ReportModel GetCaseReportViewerData(string reportName, int caseId, int caseNumber)
        {
            var reportSelectedFilter = new ReportSelectedFilter();
            reportSelectedFilter.SelectedCustomers.Add(SessionFacade.CurrentCustomer.Id);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);

            var ad = userTimeZone.GetAdjustmentRules();

            //TimeZone.CurrentTimeZone.

            var userTimeOffset = Convert.ToInt32((DateTime.UtcNow - localNow).TotalMinutes);
            userTimeOffset = userTimeOffset == 0 ? 0 : -userTimeOffset;

            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@CaseId", caseId));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@LanguageId", SessionFacade.CurrentLanguageId));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@UserId", SessionFacade.CurrentUser.Id));
            reportSelectedFilter.GeneralParameter.Add(new GeneralParameter("@UserTimeOffset", userTimeOffset));

            var reportData = _reportServiceService.GetReportData(reportName, reportSelectedFilter, SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);

            ReportModel model = new ReportModel();

            if (reportData == null || (reportData != null && !reportData.DataSets.Any()))
            {
                model = null;
            }
            else
            {
                var reportDataModel = new List<ReportDataModel>();
                if (reportData.DataSets[0].DataSet.Rows.Count > 0)
                {
                    foreach (DataRow r in reportData.DataSets[0].DataSet.Rows)
                    {
                        var row = new ReportDataModel(int.Parse(r.ItemArray[0].ToString()), r.ItemArray[1].ToString(), r.ItemArray[2].ToString(),
                                                      r.ItemArray[3].ToString(), int.Parse(r.ItemArray[4].ToString()), r.ItemArray[5].ToString());
                        if(row.FieldValue != "<p><br></p>")
                        {
                            reportDataModel.Add(row);
                        }
                    }
                }

                ReportViewer reportViewer = new ReportViewer();

                //Use Html view by data 
                /*var basePath = Request.MapPath(Request.ApplicationPath);
                var fileLocation = Path.Combine(_reportFolderName, string.Format("{0}.rdl", reportData.ReportName));
                var reportFile = Path.Combine(basePath, fileLocation);
                
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.ShowZoomControl = false;                
                reportViewer.LocalReport.ReportPath = reportFile;
                reportViewer.LocalReport.DisplayName = caseNumber.ToString();

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("CaseId", caseId.ToString()));
                parameters.Add(new ReportParameter("LanguageId", SessionFacade.CurrentLanguageId.ToString()));
                parameters.Add(new ReportParameter("UserId", SessionFacade.CurrentUser.Id.ToString()));
                reportViewer.LocalReport.SetParameters(parameters);
                
                reportViewer.LocalReport.Refresh();
                foreach (var dataSet in reportData.DataSets)
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataSet.DataSetName, dataSet.DataSet));

                model.Report = reportViewer;*/
                model.ReportData = reportDataModel;
            }

            return model;
        }
        #endregion 

    }
}