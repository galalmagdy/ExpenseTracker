using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{

    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Category { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
