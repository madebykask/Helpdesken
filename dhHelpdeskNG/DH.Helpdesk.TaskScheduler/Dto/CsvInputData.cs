using System;
using System.Collections.Generic;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    public class CsvInputData
    {
        public CsvInputData()
        {
            InputHeaders = new List<string>();
            InputColumns = new List<Tuple<string, Dictionary<string, string>>>();
        }

        public IList<string> InputHeaders { get; set; }
        public IList<Tuple<string, Dictionary<string, string>>> InputColumns { get; set; }
    }

}
