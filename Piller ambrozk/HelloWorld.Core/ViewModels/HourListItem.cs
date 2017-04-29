using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Core.ViewModels
{
    public class HourListItem
    {
        public HourListItem(string Hour)
        {
            this.Hour = Hour;
        }

        public string Hour { get; set; }
    }
}
