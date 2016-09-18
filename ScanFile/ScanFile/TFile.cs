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

    }
}
