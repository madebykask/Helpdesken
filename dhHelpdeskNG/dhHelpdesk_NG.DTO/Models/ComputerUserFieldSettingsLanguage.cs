
namespace DH.Helpdesk.BusinessData.Models
{
    public class ComputerUserFieldSettingsLanguageModel
    {
        public int Id { get; set; }
        public int Language_Id { get; set; }
        public string FieldHelp { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }    
    }

    public class CustomComputerUserFieldSettingsLanguage : ComputerUserFieldSettingsLanguageModel
    {        
        public int? CustomerId { get; set; }
    }


}
