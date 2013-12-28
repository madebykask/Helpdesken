namespace dhHelpdesk_NG.DTO.DTOs.Problem.Input
{
    using System;

    public sealed class NewProblemDto
    {
        public NewProblemDto(string name, string description, int? responsibleUserId, string inventoryNumber, bool showOnStartPage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(inventoryNumber))
            {
                throw new ArgumentNullException("inventoryNumber", "Value cannot be null or empty.");
            }

            if (responsibleUserId.HasValue && responsibleUserId.Value <= 0)
            {
                throw new ArgumentNullException("responsibleUserId", "Value must be more than zero.");
            }

            this.Name = name;
            this.Description = description;
            this.ResponsibleUserId = responsibleUserId;
            this.InventoryNumber = inventoryNumber;
            this.ShowOnStartPage = showOnStartPage;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ResponsibleUserId { get; set; }

        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }
    }
}
