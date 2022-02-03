using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IQueryAggr
    {
        public bool CreateRecord (TableAggregations newRecord);
        public void UpdateRecord(TableAggregations record);
        public void DeleteRecord(int id);
        public List<TableAggregations> GetAggrByOne(string param1);
        public List<TableAggregations> GetAggrByTwo(string param1, string param2);
        public List<TableAggregations> GetAggrByThree(string param1, string param2, string param3);
    }
}
