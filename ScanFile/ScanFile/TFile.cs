using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public class TFile
    {
        public TFile(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }
        public string Name { get; set; }
        public string FullName { get; set; }

        public double Length { get; set; }

        public double Width { get; set; }

        public int QTY { get; set; }

        public DateTime PrintTime { get; set; }

        public String Type { get; set; }

        public Double UnitPrice { get; set; }
    }
}
