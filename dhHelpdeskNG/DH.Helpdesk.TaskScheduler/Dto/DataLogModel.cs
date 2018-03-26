using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    public class DataLogModel
    {
        public DataLogModel()
        {
            RowsData = new List<RowData>();
        }

        public IList<RowData> RowsData { get; set; }

        public void Add(int id, string logData)
        {
            var _curRow = RowsData.FirstOrDefault(d => d.Id == id);
            if(_curRow == null)
            {
                var newRow = new RowData
                {
                    Id = id                    
                };

                newRow.Data.Add(logData);
                RowsData.Add(newRow);
            }
            else
            {
                _curRow.Data.Add(logData);
            }
        }

    }

    public class RowData
    {
        public RowData()
        {
            Data = new List<string>();
        }
        public int Id { get; set; }

        public IList<string> Data { get; set; }
    }
}
