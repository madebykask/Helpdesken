using System;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.Common.Extensions.ModuleName
{
    public static class ModuleNameExtensions
    {
        public static string ToModuleName(this CaseFileType type)
        {
            switch (type)
            {
                case CaseFileType.CaseFile:
                    return Enums.ModuleName.Cases;
                case CaseFileType.LogExternal:
                    return Enums.ModuleName.Log;
                case CaseFileType.LogInternal:
                    return Enums.ModuleName.LogInternal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
