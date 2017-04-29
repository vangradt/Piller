using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using ReactiveUI;
using ViewModels;
using System.Diagnostics;
using System;
using Android.Util;
using MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;

namespace HelloWorld.Core.ViewModels
{
    public class FirstViewModel : MvxViewModel
    {
        private ObservableCollection<ListItem> listitems;
        public ObservableCollection<ListItem> ListItems
        {
            get { return listitems; }
            set { listitems = value; RaisePropertyChanged(() => ListItems); }
        }

        public ReactiveCommand NavigateCommand { get; set; }

        private MvxSubscriptionToken _myToken;
        public FirstViewModel(IMvxMessenger messenger)
        {
            if (listitems == null)
            {
                this.ListItems = new ObservableCollection<ListItem>();
                //this.ListItems.Add(new ListItem("aaa", "abab", "bbb"));
            }
            _myToken = messenger.Subscribe<MyMessage>(OnMyMessageArrived);
            this.NavigateCommand = ReactiveCommand.Create(() => this.ShowViewModel<SecondViewModel>());
            this.NavigateCommand.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex));
        }

        private void OnMyMessageArrived(MyMessage p_myMessage)
        {
            ListItems.Add(p_myMessage.ListItem);
        }
    }
}
