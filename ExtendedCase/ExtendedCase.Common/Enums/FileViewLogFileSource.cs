using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedCase.Common.Enums
{
    public enum FileViewLogFileSource
    {
        Helpdesk = 5,
        Selfservice = 6,
        WebApi = 7
    }

    public enum FileViewLogOperation
    {
        Legacy = 0, // TODO: Regard old as view?
        View = 1,
        Delete = 2,
        Add = 3,
        AddTemporary = 4
    }
}
