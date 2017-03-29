using System;

namespace DH.Helpdesk.Domain.Cases
{   
    public class WorkFlowEntity : Entity
    {        

        public int Customer_Id { get; set; }

        public string ItemCaption { get; set; }

        public int Status { get; set; }

        public int User_Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}