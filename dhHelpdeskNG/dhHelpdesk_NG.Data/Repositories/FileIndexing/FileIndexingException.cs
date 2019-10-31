using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories.FileIndexing
{
	public class FileIndexingException : Exception 
	{
		public FileIndexingServiceType ServiceType { get; set; }

		public FileIndexingException(FileIndexingServiceType serviceType, Exception innerException) : base($"Failed to load { serviceType } service", innerException)
		
		{
			ServiceType = serviceType;
		}
	}
	public enum FileIndexingServiceType { IndexingService, WindowsSearch, NotResolved }
}
