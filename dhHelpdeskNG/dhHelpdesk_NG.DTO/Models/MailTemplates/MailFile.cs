namespace DH.Helpdesk.BusinessData.Models.MailTemplates
{
    public class MailFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsInternal { get; set; }

        public static MailFile CreateInternal(string fileName, string filePath)
        {
            return new MailFile()
            {
                FileName = fileName,
                FilePath = filePath,
                IsInternal = true
            };
        }
    }
}