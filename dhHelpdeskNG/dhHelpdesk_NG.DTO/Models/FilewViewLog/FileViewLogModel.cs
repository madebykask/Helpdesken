using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.FilewViewLog
{
	public enum FileViewLogOperation
	{
		Legacy = 0, // TODO: Regard old as view?
		Viewed = 1,
		Deleted = 2,
		Added = 3,
		AddedTemporary = 4
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
