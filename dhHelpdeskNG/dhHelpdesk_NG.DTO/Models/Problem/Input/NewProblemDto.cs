﻿namespace DH.Helpdesk.BusinessData.Models.Problem.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewProblemDto : INewBusinessModel
    {
        public NewProblemDto(string name, string description, int? responsibleUserId, string inventoryNumber, bool showOnStartPage, int customerId, DateTime? finishingDate)
        {
            this.Name = name;
            this.Description = description;
            this.ResponsibleUserId = responsibleUserId;
            this.InventoryNumber = inventoryNumber;
            this.ShowOnStartPage = showOnStartPage;
            this.CustomerId = customerId;
            this.FinishingDate = finishingDate;
        }

        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public string Description { get; set; }

        [IsId]
        public int? ResponsibleUserId { get; set; }

        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }

        public int CustomerId { get; set; }

        public DateTime? FinishingDate { get; set; }
    }
}
