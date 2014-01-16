namespace dhHelpdesk_NG.Web.Models.Problem
{
    using System.ComponentModel.DataAnnotations;

    public class ProblemEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int ProblemNumber { get; set; }

        public string Description { get; set; }

        public int? ResponsibleUserId { get; set; }

        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool IsFinished { get; set; }

        public bool IsExistConnectedCases { get; set; }
    }
}