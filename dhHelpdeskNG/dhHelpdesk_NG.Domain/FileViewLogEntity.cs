namespace DH.Helpdesk.Domain
{
    using global::System;
       
    public class FileViewLogEntity : Entity
    {
        public FileViewLogEntity()
        {
            
        }

        public int Case_Id { get; set; }

        public int User_Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int FileSource { get; set; }

        public DateTime CreatedDate { get; set; }

		public int? Operation { get; set; }
	}
}
