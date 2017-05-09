using Android.App;
using Android.OS;
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Piller.Resources;
using MvvmCross.Binding.BindingContext;
using Android.Views;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using System;
using System.Windows.Input;
using System.Reactive;
using MvvmCross.Binding.Droid.Views;
using Android.Opengl;
using Piller.Droid.BindingConverters;

namespace Piller.Droid.Views
{
    [Activity]
	public class MedicationDosageView : MvxAppCompatActivity<MedicationDosageViewModel>
	{
		EditText nameText;
		EditText dosageText;

		Button deleteBtn;
		Button daysBtn;
		Button timePicker;

		CheckBox monday;
		CheckBox tuesday;
		CheckBox wednesday;
		CheckBox thursday;
		CheckBox friday;
		CheckBox saturday;
		CheckBox sunday;

        MedicationDosageTimeLayout hoursList;

		protected override void OnCreate(Bundle bundle)
		{

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.MedicationDosageView);

			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar(toolbar);

			//SupportActionBar.Title = AppResources.MedicationDosageViewModel_Title;
			nameText = FindViewById<EditText>(Resource.Id.NameEditText);
			dosageText = FindViewById<EditText>(Resource.Id.DosageEditText);

			monday = FindViewById<CheckBox>(Resource.Id.mondayCheckBox);
			tuesday = FindViewById<CheckBox>(Resource.Id.tuesdayCheckBox);
			wednesday = FindViewById<CheckBox>(Resource.Id.wednesdayCheckBox);
			thursday = FindViewById<CheckBox>(Resource.Id.thursdayCheckBox);
			friday = FindViewById<CheckBox>(Resource.Id.fridayCheckBox);
			saturday = FindViewById<CheckBox>(Resource.Id.saturdayCheckBox);
			sunday = FindViewById<CheckBox>(Resource.Id.sundayCheckBox);

			deleteBtn = FindViewById<Button>(Resource.Id.deleteBtn);
			daysBtn = FindViewById<Button>(Resource.Id.everyDayBtn);
            hoursList = FindViewById<MedicationDosageTimeLayout>(Resource.Id.notificationHours);
			timePicker = FindViewById<Button>(Resource.Id.time_picker);

          
            hoursList.ItemTemplateId = Resource.Layout.time_item;


			//obsluga usuwania - jedna z kilku mozliwosci
			//wcisniecie przyscisku delete spowoduje wywolanie na adapterze komendy z usuwana godzina (implementacja w MedicationDosageTimeListAdapter
            var hourAdapter = (MedicationDosageTimeListAdapter)hoursList.Adapter;//dialog tworzymy i pokazujemy z kodu
            hourAdapter.DeleteRequested.Subscribe(time => this.ViewModel.DosageHours.Remove(time));


			//aby ui sie odswiezyl, lista godzin powinna być jakimś typem NotifyCollectionChanged (np. ReactiveList)
			//w samym UI można użyć MvxLinearLayout, który działa podobnie do listy,ale nie spowoduje scrolla w scrollu
			//wtedy właściwość Times bindujemy to tego komponentu
			timePicker.Click += (o, e) =>
			{
				TimePickerDialog timePickerFragment = new TimePickerDialog(
                       this,
                    (s, args) =>
                    {
                        // handler jest wołany dwukrotnie: raz bo wybrana została godzina, drugi bo picker został zamknięty - ten musimy zignorować
	                    if (((TimePicker)s).IsShown)
	                        this.ViewModel.DosageHours.Add(new TimeSpan(args.HourOfDay, args.Minute, 0));
                    },
					   12,
					   00,
					   true
				   );
				timePickerFragment.Show();
			};

			SetBinding();
		}



		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			this.MenuInflater.Inflate(Resource.Menu.dosagemenu, menu);
			var saveItem = menu.FindItem(Resource.Id.action_save);

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			//sprawdzamy, czy przycisk ma id zdefiniowane dla Save, i czy Save mozna wywolac (to na przyszlosc, gdy bedzie walidacja)
			// jak tak - odpalamy komendę. To dziwne Subscribe na końcu do wymóg ReactiveUI7
			if (item.ItemId == Resource.Id.action_save && ((ICommand)this.ViewModel.Save).CanExecute(null))
				this.ViewModel.Save.Execute(Unit.Default).Subscribe();
			return base.OnOptionsItemSelected(item);
		}

		private MvxFluentBindingDescriptionSet<MedicationDosageView, MedicationDosageViewModel> bindingSet;
        private void SetBinding()
        {
            bindingSet = this.CreateBindingSet<MedicationDosageView, MedicationDosageViewModel>();

            //sposob na bezposrednie sluchanie observable. W momencie, gdy CanExecute sie zmieni wykona sie kod z Subscribe
            this.ViewModel.Delete.CanExecute.Subscribe(canExecute => deleteBtn.Visibility = canExecute ? ViewStates.Visible : ViewStates.Gone);

            bindingSet.Bind(this.SupportActionBar)
                      .To(x => x.MedicationName)
                      .For(v => v.Title)
                      .WithConversion(new InlineValueConverter<string, string>(medicationName =>
                      {
                          if (string.IsNullOrEmpty(medicationName))
                              return "";//this.ViewModel.Id.HasValue ? "" : AppResources.MedicationDosageViewModel_Title;
                          return medicationName;
                      }));

			bindingSet.Bind(nameText)
					  .To(x => x.MedicationName);

			bindingSet.Bind(dosageText)
				.To(vm => vm.MedicationDosage);
			bindingSet.Bind(deleteBtn)
				.To(vm => vm.Delete);

			bindingSet.Bind(monday)
				.For(x => x.Checked)
				.Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
				.To(vm => vm.Monday);
			bindingSet.Bind(tuesday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Tuesday);
			bindingSet.Bind(wednesday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Wednesday);
			bindingSet.Bind(thursday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Thurdsday);
			bindingSet.Bind(friday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Friday);
			bindingSet.Bind(saturday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Saturday);
			bindingSet.Bind(sunday)
			   .For(x => x.Checked)
			   .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
			   .To(vm => vm.Sunday);

			bindingSet.Bind(daysBtn)
				.To(vm => vm.SelectAllDays);
			
            bindingSet.Bind(hoursList)
				.For(x => x.ItemsSource)
                      .To(vm => vm.DosageHours);
			bindingSet.Apply();



		}

	}
}