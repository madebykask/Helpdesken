

using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class WorkFlowModel: INewBusinessModel
    {
        public WorkFlowModel()
        {

        }

        public int Id { get; set; }

        public int Customer_Id { get; set; }

        public string ItemCaption { get; set; }        

        public bool IsActive { get; set; }

        public int User_Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

    }


}
