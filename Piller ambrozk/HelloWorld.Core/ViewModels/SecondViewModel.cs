using MvvmCross.Core.ViewModels;
using ReactiveUI;
using HelloWorld.Core.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;
using Android.Util;
using Java.Lang;

namespace ViewModels
{
    public class SecondViewModel : MvxViewModel
    {
        //list of string corresponding to checked days
        private List<string> dayslist = new List<string>();
        //messenger for communication between first and second view
        private readonly IMvxMessenger _messenger;
        private ListItem listitem;
        private HourListItem houritem;

        private string hour;
        public string Hour
        {
            get { return hour; }
            set
            {
                this.SetProperty(ref this.hour, value);
            }
        }

        private ObservableCollection<HourListItem> houritems = new ObservableCollection<HourListItem>();
        public ObservableCollection<HourListItem> HourItems
        {
            get { return houritems; }
            set { this.SetProperty(ref houritems, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                this.SetProperty(ref this.name, value);
            }
        }

        private string dosage;
        public string Dosage
        {
            get { return dosage; }
            set
            {
                this.SetProperty(ref this.dosage, value);
            }
        }

        private bool pon;
        public bool Pon
        {
            get { return pon; }
            set
            {
                this.SetProperty(ref this.pon, value);
            }
        }

        private bool wt;
        public bool Wt
        {
            get { return wt; }
            set
            {
                this.SetProperty(ref this.wt, value);
            }
        }

        private bool sr;
        public bool Sr
        {
            get { return sr; }
            set
            {
                this.SetProperty(ref this.sr, value);
            }
        }

        private bool czw;
        public bool Czw
        {
            get { return czw; }
            set
            {
                this.SetProperty(ref this.czw, value);
            }
        }

        private bool pt;
        public bool Pt
        {
            get { return pt; }
            set
            {
                this.SetProperty(ref this.pt, value);
            }
        }

        private bool sb;
        public bool Sb
        {
            get { return sb; }
            set
            {
                this.SetProperty(ref this.sb, value);
            }
        }

        private bool nd;
        public bool Nd
        {
            get { return nd; }
            set
            {
                this.SetProperty(ref this.nd, value);
            }
        }

        //get list of string corresponding to checked days
        private string getDays()
        {
            if (pon)
                dayslist.Add("PON");
            if (wt)
                dayslist.Add("WT");
            if (sr)
                dayslist.Add("SR");
            if (czw)
                dayslist.Add("CZW");
            if (pt)
                dayslist.Add("PT");
            if (sb)
                dayslist.Add("SB");
            if (nd)
                dayslist.Add("ND");

            return daysToString(); ;
        }

        //get days as (PON, WT, ..)
        private string daysToString()
        {
            StringBuilder sb = new StringBuilder();

            string delim = "";
            sb.Append("(");
            foreach (string i in dayslist)
            {
                sb.Append(delim).Append(i);
                delim = ",";
            }
            sb.Append(")");

            return sb.ToString();
        }

        //get hours as 10:00, 11:00, ...
        private string hoursToString()
        {
            StringBuilder sb = new StringBuilder();
            string delim = "";

            foreach (HourListItem i in HourItems)
            {
                sb.Append(delim).Append(i.Hour);
                delim = ", ";
            }
            return sb.ToString();
        }

        private void onClose()
        {
            if (name != null && dosage != null)
            {
                listitem = new ListItem(name, dosage, getDays(), hoursToString());
                _messenger.Publish<MyMessage>(new MyMessage(this, listitem));
            }
            Close(this);
        }

        public ReactiveCommand NavigateAndPassDataCommand { get; set; }

        public SecondViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            this.NavigateAndPassDataCommand = ReactiveCommand.Create(() =>
                onClose());
        }

        //add hour to a list of hours
        MvxCommand _AddHour;

        public IMvxCommand AddHour
        {
            get { return new MvxCommand(Add); }
        }


        void Add()
        {
            if (hour != null)
            {
                houritem = new HourListItem(hour);
                HourItems.Add(houritem);
            }
        }
    }

}
