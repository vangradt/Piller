using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace HelloWorld.Core.ViewModels
{
    public class MyMessage : MvxMessage
    {
        public MyMessage(object sender, ListItem p_item3) : base(sender)
        {
            ListItem = p_item3;
        }

        public ListItem ListItem { private set; get; }
    }
}
