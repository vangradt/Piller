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
using MvvmCross.Binding.BindingContext;
using Piller.ViewModels;
using MvvmCross.Platform.Converters;
using Android.Graphics;
using System.Globalization;
using System.IO;
using Converters;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Windows.Input;
using System.Reactive;

namespace Piller.Droid.Views
{
    [Activity]
    public class PhotoGalleryView : MvxAppCompatActivity<PhotoGalleryViewModel>
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PhotoGalleryView);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var takePhoto = FindViewById<Button>(Resource.Id.take_photo);
            var selectPhoto = FindViewById<Button>(Resource.Id.select_photo);
            var photo = FindViewById<ImageView>(Resource.Id.photo);

            var bSet = this.CreateBindingSet<PhotoGalleryView, PhotoGalleryViewModel>();

            bSet.Bind(takePhoto)
                        .To(vm => vm.TakePhotoCommand);

            bSet.Bind(selectPhoto)
                        .To(vm => vm.SelectPhotoCommand);

            bSet.Bind(photo)
                        .To(vm => vm.Bytes);

            bSet.Apply();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.dosagemenu, menu);
            var saveItem = menu.FindItem(Resource.Id.action_save);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_save && ((ICommand)this.ViewModel.Save).CanExecute(null))
                this.ViewModel.Save.Execute(Unit.Default).Subscribe();
            return base.OnOptionsItemSelected(item);
        }
    }

}