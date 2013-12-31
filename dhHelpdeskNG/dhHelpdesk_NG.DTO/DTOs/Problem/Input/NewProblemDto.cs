namespace dhHelpdesk_NG.DTO.DTOs.Problem.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewProblemDto
    {
        public NewProblemDto(string name, string description, int? responsibleUserId, string inventoryNumber, bool showOnStartPage, int customerId)
        {
            this.Name = name;
            this.Description = description;
            this.ResponsibleUserId = responsibleUserId;
            this.InventoryNumber = inventoryNumber;
            this.ShowOnStartPage = showOnStartPage;
            this.CustomerId = customerId;
        }

        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public string Description { get; set; }

        [IsId]
        public int? ResponsibleUserId { get; set; }

        [NotNullAndEmpty]
        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }

        public int CustomerId { get; set; }
    }
}
