using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain.Computers
{
    public class WorkstationTabSetting: Entity
    {
        public int Customer_Id { get; set; }
        public string TabField { get; set; }
        public bool Show { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkstationTabSettingLanguage> WorkstationTabSettingLanguages { get; set; }
    }
}