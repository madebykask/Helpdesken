namespace DH.Helpdesk.Web.Models.Inventory.EditModel
{
    using DH.Helpdesk.Common.ValidationAttributes;
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

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ComputerId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Text")]
        public string LogText { get; set; }
    }
}