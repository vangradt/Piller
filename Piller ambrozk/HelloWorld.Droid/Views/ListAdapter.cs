using System;
using MvvmCross.Binding.Droid.Views;
using HelloWorld.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using Android.Widget;
using HelloWorld.Droid;

namespace Views
{
    public class ListAdapter : MvxAdapter
    {
        public ListAdapter(Android.Content.Context context) : base(context)
        {
        }

        public ListAdapter(Android.Content.Context context, MvvmCross.Binding.Droid.BindingContext.IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        public ListAdapter(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, ListItem>();

            var name = view.FindViewById<TextView>(Resource.Id.label_name);
            var surname = view.FindViewById<TextView>(Resource.Id.label_dosage);
            var days = view.FindViewById<TextView>(Resource.Id.days);
            var hours = view.FindViewById<TextView>(Resource.Id.hours);

            bset.Bind(name)
                .To(x => x.Name);

            bset.Bind(surname)
                .To(x => x.Dosage);

            bset.Bind(days)
                .To(x => x.Days);

            bset.Bind(hours)
                .To(x => x.Hours);

            bset.Apply();
            return view;
        }
    }
}
