namespace DH.Helpdesk.BusinessData.Models.Contract
{
    
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using System;
    using System.Collections.Generic;


    public class ContractInputModel : INewBusinessModel
    {
        public ContractInputModel() { }

        public ContractInputModel(
                int id,
                int customerId,
                int changedByUserId,       
                int categoryId,
                int? supplierId,
                int? departmentId,
                int? responsibleUserId,
                int? followUpResponsibleUserId,
                string contractNumber,
                DateTime? contractStartDate,
                DateTime? contractEndDate,
                int noticeTime,
                bool finished,
                bool running,
                int followUpInterval,
                string other,
                DateTime? noticeDate,
                DateTime createdDate,
                DateTime changedDate,
                Guid contractGuid, 
                List<string> files)
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.ChangedByUser_Id = changedByUserId;       
            this.CategoryId = categoryId;
            this.SupplierId = supplierId;
            this.DepartmentId = departmentId;
            this.ResponsibleUserId = responsibleUserId;
            this.FollowUpResponsibleUserId = followUpResponsibleUserId;
            this.ContractNumber = contractNumber;
            this.ContractStartDate = contractStartDate;
            this.ContractEndDate = contractEndDate;
            this.NoticeTime = noticeTime;
            this.Finished = finished;
            this.Running = running;
            this.FollowUpInterval = followUpInterval;
            this.Other = other;
            this.NoticeDate = noticeDate;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.ContractGUID = contractGuid;
            this.Files = files;
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ChangedByUser_Id { get; set; }      

        public int CategoryId { get; set; }

        public int? SupplierId { get; set; }

        public int? DepartmentId { get; set; }

        public int? ResponsibleUserId { get; set; }

        public int? FollowUpResponsibleUserId { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int NoticeTime { get; set; }

        public bool Finished { get; set; }

        public bool Running { get; set; }

        public int FollowUpInterval { get; set; }

        public string Other { get; set; }

        public DateTime? NoticeDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public Guid ContractGUID { get; set; }

        public List<string> Files { get; set; }

    }
}