﻿namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class Department : Entity, ICustomerEntity, IActiveEntity
    {
        public int AccountancyAmount { get; set; }
        public int Charge { get; set; }
        public int ChargeMandatory { get; set; }
        public int? Country_Id { get; set; }
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsEMailDefault { get; set; }
        public int? Region_Id { get; set; }
        public int ShowInvoice { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string HeadOfDepartment { get; set; }
        public string HeadOfDepartmentEMail { get; set; }
        public string HomeDirectory { get; set; }
        public string Path { get; set; }
        public string SearchKey { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }
        public int? HolidayHeader_Id { get; set; }
        public int? WatchDateCalendar_Id { get; set; }
        public int OverTimeAmount { get; set; }
        public string Code { get; set; }

        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual HolidayHeader HolidayHeader { get; set; }
        public virtual WatchDateCalendar WatchDateCalendar { get; set; }
    }
}