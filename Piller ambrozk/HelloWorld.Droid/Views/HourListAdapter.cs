using System;
using MvvmCross.Binding.Droid.Views;
using HelloWorld.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using Android.Widget;
using HelloWorld.Droid;

namespace Views
{
    public class HourListAdapter : MvxAdapter
    {
        public HourListAdapter(Android.Content.Context context) : base(context)
        {
        }

        public HourListAdapter(Android.Content.Context context, MvvmCross.Binding.Droid.BindingContext.IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        public HourListAdapter(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, HourListItem>();

            var hour = view.FindViewById<TextView>(Resource.Id.label_hour);

            bset.Bind(hour)
                .To(x => x.Hour);

            bset.Apply();
            return view;
        }
    }
}
