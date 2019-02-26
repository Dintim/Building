using Building.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Model
{
    public class Payment
    {
        public IWorker worker { get; set; }
        public double totalSalary { get; set; }

        public Payment()
        {
            totalSalary = 0;
        }
        
    }
}
