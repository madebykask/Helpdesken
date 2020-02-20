using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
	public class FileUploadExtensionsModel
	{
		public bool UseFileExtensionWhiteList { get; set; }
		public string FileExtensions { get; set; }
	}
}