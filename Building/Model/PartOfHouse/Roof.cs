using Building.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Model.PartOfHouse
{
    public enum RoofTypes { flat, sloped}
    public class Roof:IPart
    {
        public string name { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        public int order { get; set; }
        public ITask task { get; set; }
        public RoofTypes roofTypes { get; set; }

        public string getInfo()
        {
            string info = string.Format("Наименование: {0} {1} ({2})\nЦена: {3}",
                name, roofTypes, count, price);
            return info;
        }
    }
}
