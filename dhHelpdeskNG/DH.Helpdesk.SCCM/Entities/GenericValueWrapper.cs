using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{
    public class GenericValueWrapper<T>
    {
        public List<T> value = new List<T>();
    }
}
