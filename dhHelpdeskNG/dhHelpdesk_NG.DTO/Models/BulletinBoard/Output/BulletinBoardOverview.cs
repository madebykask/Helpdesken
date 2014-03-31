using System;

namespace DH.Helpdesk.BusinessData.Models.BulletinBoard.Output
{
    public sealed class BulletinBoardOverview
    {
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
    }
}