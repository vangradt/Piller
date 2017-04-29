using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Core.ViewModels
{
    public class ListItem
    {
        public ListItem(string Name, string Dosage, string Days, string Hours)
        {
            this.Name = Name;
            this.Dosage = Dosage;
            this.Days = Days;
            this.Hours = Hours;
        }

        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Days { get; set; }
        public string Hours { get; set; }
    }
}
