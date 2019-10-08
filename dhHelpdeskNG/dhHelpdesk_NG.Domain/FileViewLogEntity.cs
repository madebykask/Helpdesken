using DH.Helpdesk.Common.Enums.FileViewLog;

namespace DH.Helpdesk.Domain
{
    using global::System;
       
    public class FileViewLogEntity : Entity
    {
        public FileViewLogEntity()
        {
            
        }

        public int Case_Id { get; set; }

        public int? User_Id { get; set; }
        public string UserName { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public FileViewLogFileSource FileSource { get; set; }

        public DateTime CreatedDate { get; set; }

		public FileViewLogOperation? Operation { get; set; }
        public virtual Case Case { get; set; }
        public virtual User User { get; set; }
    }
}
