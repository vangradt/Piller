using System;
using MvvmCross.Core.ViewModels;
using RxUI = ReactiveUI;
using System.Reactive;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;
using Acr.UserDialogs;
using Piller.Resources;
using ReactiveUI;
using MvvmCross.Plugins.Messenger;
using Android.App;

namespace Piller.ViewModels
{
    public class MedicationDosageViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

        private byte[] bytes;
        public byte[] Bytes
        {
            get { return this.bytes; }
            set { this.SetProperty(ref this.bytes, value); }
        }

        private byte[] bytes_pill;
        public byte[] Bytes_pill
        {
            get { return this.bytes_pill; }
            set { this.SetProperty(ref this.bytes_pill, value); }
        }

        private byte[] bytes_rx;
        public byte[] Bytes_rx
        {
            get { return this.bytes_rx; }
            set { this.SetProperty(ref this.bytes_rx, value); }
        }

        //identyfikator rekordu, uzywany w trybie edycji
        private int? id;
        public int? Id { 
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }

        string medicationName;
        public string MedicationName
        {
            get { return medicationName; }
            set { this.SetProperty(ref medicationName, value); }
        }


        string medicationDosage;
        public string MedicationDosage
        {
            get { return medicationDosage; }
            set { this.SetProperty(ref medicationDosage, value); }
        }


        private bool monday;
        public bool Monday
        {
            get { return monday; }
            set { this.SetProperty(ref monday, value); }
        }

        private bool tuesday;
        public bool Tuesday
        {
            get { return tuesday; }
            set { this.SetProperty(ref tuesday, value); }
        }

        private bool wednesday;
        public bool Wednesday
        {
            get { return wednesday; }
            set { this.SetProperty(ref wednesday, value); }
        }

        private bool thursday;
        public bool Thursday
        {
            get { return thursday; }
            set { this.SetProperty(ref thursday, value); }
        }

        private bool friday;
        public bool Friday
        {
            get { return friday; }
            set { this.SetProperty(ref friday, value); }
        }

        private bool saturday;
        public bool Saturday
        {
            get { return saturday; }
            set { this.SetProperty(ref saturday, value); }
        }

        private bool sunday;
        public bool Sunday
        {
            get { return sunday; }
            set { this.SetProperty(ref sunday, value); }
        }


        private RxUI.ReactiveList<TimeSpan> dosageHours;
        public RxUI.ReactiveList<TimeSpan> DosageHours
        {
            get { return this.dosageHours; }
            set { SetProperty(ref dosageHours, value); }
        }

        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }
        public RxUI.ReactiveCommand<MedicationDosage, bool> Delete { get; set; }
        public RxUI.ReactiveCommand SelectAllDays { get; set; }

        public MedicationDosageViewModel()
        {
            this.DosageHours = new RxUI.ReactiveList<TimeSpan>();

            var canSave = this.WhenAny(vm => vm.MedicationName,
                                       vm => vm.MedicationDosage,
                                       vm => vm.Monday,
                                       vm => vm.Tuesday,
                                       vm => vm.Wednesday,
                                       vm => vm.Thursday,
                                       vm => vm.Friday,
                                       vm => vm.Saturday,
                                       vm => vm.Sunday,
                                       vm => vm.DosageHours.Count,
                                       (name, dosage, monday, tuesday, wednesday, thursday, friday, saturday, sunday, hours) => 
                                       !String.IsNullOrWhiteSpace(name.Value) && 
                                       !String.IsNullOrWhiteSpace(dosage.Value) &&
                                       (monday.Value ||
                                       tuesday.Value ||
                                       wednesday.Value ||
                                       thursday.Value ||
                                       friday.Value ||
                                       saturday.Value ||
                                       sunday.Value
                                       ) &&
                                       hours.Value != 0
                );

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool>(async _ =>
            {

                var dataRecord = new MedicationDosage
                {
                    Id = this.Id,
                    Name = this.MedicationName,
                    Dosage = this.MedicationDosage,
                    Days =
                        (this.Monday ? DaysOfWeek.Monday : DaysOfWeek.None)
                        | (this.Tuesday ? DaysOfWeek.Tuesday : DaysOfWeek.None)
                        | (this.Wednesday ? DaysOfWeek.Wednesday : DaysOfWeek.None)
                        | (this.Thursday ? DaysOfWeek.Thursday : DaysOfWeek.None)
                        | (this.Friday ? DaysOfWeek.Friday : DaysOfWeek.None)
                        | (this.Saturday ? DaysOfWeek.Saturday : DaysOfWeek.None)
                        | (this.Sunday ? DaysOfWeek.Sunday : DaysOfWeek.None),
                    DosageHours = this.DosageHours,
                    Bytes = this.Bytes,
                    Bytes_pill = this.Bytes_pill,
                    Bytes_rx = this.Bytes_rx
                };

                await this.storage.SaveAsync<MedicationDosage>(dataRecord);

                return true;
            }, canSave);


            

            var canDelete = this.WhenAny(x => x.Id, id => id.Value.HasValue);
            this.Delete = RxUI.ReactiveCommand.CreateFromTask<Data.MedicationDosage, bool>(async _ =>
               {
                   if (this.Id.HasValue)
                   {
                       await this.storage.DeleteByKeyAsync<MedicationDosage>(this.Id.Value);
                       return true;
                   }
                   return false;
             }, canDelete);

            this.SelectAllDays = RxUI.ReactiveCommand.Create(() => { Monday = true; Tuesday = true; Wednesday = true; Thursday = true; Friday = true; Saturday = true; Sunday = true; });


            //save sie udal, albo nie - tu dosatniemy rezultat komendy. Jak sie udal, to zamykamy ViewModel
            this.Save
                .Subscribe(result =>
                {
                    if (result)
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new DataChangedMessage(this));
                        this.Close(this);
                    }
                });

            this.Save.ThrownExceptions.Subscribe(ex =>
            {
                UserDialogs.Instance.ShowError(AppResources.MedicationDosageView_SaveError);
                // show nice message to the user

            });



            this.Delete
                .Subscribe(result =>
                {
                    if (result)
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new DataChangedMessage(this));
                        this.Close(this);
                    }
                });
        }


        public async void Init(MedicationDosageNavigation nav)
        {
            if (nav.MedicationDosageId != MedicationDosageNavigation.NewRecord)
            {
                Data.MedicationDosage item = await storage.GetAsync<Data.MedicationDosage>(nav.MedicationDosageId);
                Id = item.Id;
                MedicationName = item.Name;
                MedicationDosage = item.Dosage;
                Monday = item.Days.HasFlag(DaysOfWeek.Monday);
                Tuesday = item.Days.HasFlag(DaysOfWeek.Tuesday);
                Wednesday = item.Days.HasFlag(DaysOfWeek.Wednesday);
                Thursday = item.Days.HasFlag(DaysOfWeek.Thursday);
                Friday = item.Days.HasFlag(DaysOfWeek.Friday);
                Saturday = item.Days.HasFlag(DaysOfWeek.Saturday);
                Sunday = item.Days.HasFlag(DaysOfWeek.Sunday);
                DosageHours = new RxUI.ReactiveList<TimeSpan>(item.DosageHours);
                Bytes = item.Bytes;
                Bytes_pill = item.Bytes_pill;
                Bytes_rx = item.Bytes_rx;
            }
      
        }
    }
}
