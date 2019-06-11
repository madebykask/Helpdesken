using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CaseSettings : Entity
    {
        public int? Customer_Id { get; set; }
        public int ColOrder { get; set; }
        
        /// <summary>
        /// What line to use to display this column value. 
        /// Does not support this feature in v5
        /// </summary>
        public int Line { get; set; }
        public int MinWidth { get; set; }
        public string ColStyle { get; set; }

        public int? User_Id { get; set; }
        public int UserGroup { get; set; }
        public string Name { get; set; }
        public CaseSettingTypes Type { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
