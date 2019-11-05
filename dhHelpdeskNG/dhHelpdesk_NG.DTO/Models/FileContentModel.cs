using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models
{
	public class FileContentModel
	{
		public byte[] Content { get; set; }
		public string FilePath { get; set; }
	}
}
