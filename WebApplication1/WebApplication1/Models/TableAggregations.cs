using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class TableAggregations
    {
        [Key]
        public int Id { get; set; }
        public string Project { get; set; }
        public string Employee { get; set; }
        public string Date { get; set; }
        public int Hours { get; set; }
    }
}
