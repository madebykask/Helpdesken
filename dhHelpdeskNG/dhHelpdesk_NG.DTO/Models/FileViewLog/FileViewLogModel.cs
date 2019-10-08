using System;
using DH.Helpdesk.Common.Enums.FileViewLog;

namespace DH.Helpdesk.BusinessData.Models.FileViewLog
{
	public class FileViewLogModel
	{
		public int Id { get; set; }
		public int? User_Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }

		public FileViewLogFileSource FileSource { get; set; }
		public DateTime CreatedDate { get; set; }

		public int Case_Id { get; set; }

		public FileViewLogOperation Operation { get; set; }
        public string UserName { get; set; }
    }
}
