using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedCase.Models.Files
{
    public class CaseFileContent
    {
        public int Id { get; set; }
        public int CaseNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] Content { get; set; }
    }
}
