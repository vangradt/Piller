using System;
using System.Reactive;
using MvvmCross.Core.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Android.Util;

namespace Piller.ViewModels
{
	public class MedicationSummaryListViewModel : MvxViewModel
	{
		private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        MvxSubscriptionToken dataChangedSubscriptionToken;
        MvxSubscriptionToken showGallerySubscriptionToken;

        private List<MedicationDosage> medicationList;
		public List<MedicationDosage> MedicationList
		{
			get { return medicationList; }
			set { SetProperty(ref medicationList, value); }
		}


		public ReactiveCommand<Unit, bool> AddNew { get; }
		public ReactiveCommand<Data.MedicationDosage, Unit> Edit { get; }

		public MedicationSummaryListViewModel()
		{
			AddNew = ReactiveCommand.Create(() => this.ShowViewModel<MedicationDosageViewModel>());
			Edit = ReactiveCommand.Create<Data.MedicationDosage>((item) =>
			 {
                this.ShowViewModel<MedicationDosageViewModel>(new MedicationDosageNavigation { MedicationDosageId = item.Id.Value });
			 });

            dataChangedSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<DataChangedMessage>(async mesg => await Init());
            showGallerySubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ShowGalleryMessage>(mesg => ShowGallery(mesg));

        }

        private void ShowGallery(ShowGalleryMessage mesg)
        {
            this.ShowViewModel<PhotoGalleryViewModel>(new MedicationDosageNavigation { MedicationDosageId = mesg.MedicationDosageItem.Id.Value });
        }

        public async Task Init()
        {
	
			var items = await storage.List<MedicationDosage>();
            if (items != null)
            {
                MedicationList = items;
            }
            else
                MedicationList = new List<MedicationDosage>();
        }
	
		
	}
}