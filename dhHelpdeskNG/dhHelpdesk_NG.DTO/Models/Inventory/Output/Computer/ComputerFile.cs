namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class ComputerFile
    {
        public ComputerFile()
        {
        }

        public ComputerFile(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public string FileName { get; set; }       
        public byte[] Content { get; set; }
    }
}