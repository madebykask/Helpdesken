namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ComputerLogModel
    {
        public ComputerLogModel()
        {
        }

        public ComputerLogModel(int id, int computerId, string logText)
        {
            this.Id = id;
            this.ComputerId = computerId;
            this.LogText = logText;
        }

        public int Id { get; set; }

        [IsId]
        public int ComputerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Text")]
        public string LogText { get; set; }

        public bool IsForDialog { get; set; }

        public ComputerLog CreateBusinessModel()
        {
            return ComputerLog.CreateNew(
                this.ComputerId,
                SessionFacade.CurrentUser.Id,
                string.Empty,
                this.LogText,
                DateTime.Now);
        }
    }
}