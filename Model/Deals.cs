using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListManager.DB
{
    internal class Deals
    {
        public int Id { get; set; }
        public string Case { get; set; }
        public int Priority { get; set; }
        public DateTime Execution { get; set; }

        public Deals()
        {
            Case = "";
        }

        public override string ToString()
        {
            return $"{Id} - {Case} - {Priority} - {Execution}";
        }
    }
}
