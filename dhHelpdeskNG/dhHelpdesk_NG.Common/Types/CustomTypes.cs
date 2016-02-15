using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Common.Types
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }

    public class Utf16StringWriter : StringWriter
    {
        
    }
    

}
