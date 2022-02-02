using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Keyless]
    public class TableAggregations
    {
        public string Project { get; set; }
        public string Employee { get; set; }
        public string Date { get; set; }
        public int Hours { get; set; }
    }
}
