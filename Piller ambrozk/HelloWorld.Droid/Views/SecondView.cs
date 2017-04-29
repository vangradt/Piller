
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HelloWorld.Droid;
using HelloWorld.Droid.Views;
using MvvmCross.Binding.BindingContext;
using ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using Android.Util;

namespace Views
{
    [Activity(Label = "SecondView")]
    public class SecondView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.SecondView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var buttonAdd = FindViewById<Button>(Resource.Id.button_new_item);
            var inputName = FindViewById<EditText>(Resource.Id.edit_1);

            var inputDosage = FindViewById<EditText>(Resource.Id.edit_2);
            var checkboxPon = FindViewById<CheckBox>(Resource.Id.checkbox_pon);
            var checkboxWt = FindViewById<CheckBox>(Resource.Id.checkbox_wt);
            var checkboxSr = FindViewById<CheckBox>(Resource.Id.checkbox_sr);
            var checkboxCzw = FindViewById<CheckBox>(Resource.Id.checkbox_czw);
            var checkboxPt = FindViewById<CheckBox>(Resource.Id.checkbox_pt);
            var checkboxSb = FindViewById<CheckBox>(Resource.Id.checkbox_sb);
            var checkboxNd = FindViewById<CheckBox>(Resource.Id.checkbox_nd);

            var buttonAddHour = FindViewById<Button>(Resource.Id.add_hour_btn);
            var hours_list = FindViewById<MvxListView>(Resource.Id.hours_list);
            var inputHour = FindViewById<EditText>(Resource.Id.edit_hour);

            hours_list.Adapter = new HourListAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);
            hours_list.ItemTemplateId = Resource.Layout.list_item_hour;

            var bSet = this.CreateBindingSet<SecondView, SecondViewModel>();

            bSet.Bind(buttonAdd)
                .To(vm => vm.NavigateAndPassDataCommand);

            bSet.Bind(inputName)
                .To(vm => vm.Name);

            bSet.Bind(inputHour)
                .To(vm => vm.Hour);

            bSet.Bind(inputDosage)
                .To(vm => vm.Dosage);

            bSet.Bind(checkboxPon)
                .To(vm => vm.Pon);

            bSet.Bind(checkboxWt)
                .To(vm => vm.Wt);

            bSet.Bind(checkboxSr)
                .To(vm => vm.Sr);

            bSet.Bind(checkboxCzw)
                .To(vm => vm.Czw);

            bSet.Bind(checkboxPt)
                .To(vm => vm.Pt);

            bSet.Bind(checkboxSb)
                .To(vm => vm.Sb);

            bSet.Bind(checkboxNd)
                .To(vm => vm.Nd);

            bSet.Bind(buttonAddHour)
                .To(vm => vm.AddHour);

            bSet.Bind(hours_list)
                .For(x => x.ItemsSource)
                .To(vm => vm.HourItems);

            bSet.Apply();
        }
    }
}
