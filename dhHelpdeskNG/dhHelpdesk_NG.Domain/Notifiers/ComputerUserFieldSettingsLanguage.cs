namespace DH.Helpdesk.Domain.Notifiers
{
    using global::System.ComponentModel.DataAnnotations;

    [Table("tblComputerUserFS_tblLanguage")]
    public class ComputerUserFieldSettingsLanguage
    {
        #region Public Properties

        [ForeignKey("ComputerUserFieldSettings_Id")]
        public virtual ComputerUserFieldSettings ComputerUserFieldSettings { get; set; }

        [Key, Column(Order = 0)]
        public int ComputerUserFieldSettings_Id { get; set; }

        [MaxLength(200)]
        public string FieldHelp { get; set; }

        [Required]
        [MaxLength(50)]
        public string Label { get; set; }

        [Key, Column(Order = 1)]
        public int Language_Id { get; set; }

        #endregion
    }
}