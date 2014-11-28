using DH.Helpdesk.BusinessData.Models.DailyReport.Input;
using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace DH.Helpdesk.Web.Models.DailyReports
{   
    public sealed class DailyReportInputModel
    {
        public DailyReportInputModel()
        { }

        public DailyReportInputModel(
                int id,
                int sent,
                string userName,
                DateTime createdDate,
                int dailyReportSubjectId,
                string dailyReportText,
                string firstName,
                string lastName,
                List<DailyReportSubjectBM> subjects)
        {
            this.Id = id;
            this.Sent = Sent;
            this.UserName = userName;
            this.DailyReportText = dailyReportText;
            this.DailyReportSubjectId = dailyReportSubjectId;
            this.CreatedDate = createdDate;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Subjects = subjects.Select(x => new SelectListItem
                {
                    Selected = (x.Id == dailyReportSubjectId?true:false),
                    Text = x.Subject,
                    Value = x.Id.ToString()
                }).ToList();  

        }

        
        public int Id { get; set; }

        public int Sent { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DailyReportSubjectId { get; set; }

        public string DailyReportText { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SelectListItem> Subjects { get; set; }        

    }
}