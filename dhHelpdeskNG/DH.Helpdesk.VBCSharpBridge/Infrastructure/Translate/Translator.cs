using DH.Helpdesk.Dal.Infrastructure.Translate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Infrastructure.Translate
{
    public sealed class Translator : ITranslator
    {
        public string Translate(string text)
        {
            return Translation.Get(text);
        }
    }
}
