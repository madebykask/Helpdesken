using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Dal.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Infrastructure.Translate
{
    public sealed class Translator : ITranslator
    {
        public string Translate(string text)
        {
            return Translation.Get(text);
        }
    }
}