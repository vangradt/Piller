using MvvmCross.Plugins.Messenger;
using Piller.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piller.ViewModels
{
    public class ShowGalleryMessage : MvxMessage
    {
        public ShowGalleryMessage(object sender, MedicationDosage item) : base(sender)
        {
            MedicationDosageItem = item;
        }
        public MedicationDosage MedicationDosageItem { private set; get; }
    }
}
