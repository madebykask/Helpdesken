using System;

namespace DH.Helpdesk.BusinessData.Models.FileViewLog
{
	public enum FileViewLogOperation
	{
		Legacy = 0, // TODO: Regard old as view?
		View = 1,
		Delete = 2,
		Add = 3,
		AddTemporary = 4
	}

	public enum FileViewLogFileSource
	{
		Helpdesk = 5,
		Selfservice = 6,
		WebApi = 7
	}

	public class FileViewLogModel
	{
		public int Id { get; set; }
		public int User_Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }

		public FileViewLogFileSource FileSource { get; set; }
		public DateTime CreatedDate { get; set; }

		public int Case_Id { get; set; }

		public FileViewLogOperation Operation { get; set; }

	}
}
