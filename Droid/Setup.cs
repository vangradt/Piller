using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Services;
using Acr.UserDialogs;
using System;
using MvvmCross.Platform.Droid.Platform;
using Piller.Services;
using Piller.Droid.Services;

namespace Piller.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new PillerApp();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override MvvmCross.Droid.Views.IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var mvxFragmentsPresenter = new MvxFragmentsPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
            Mvx.RegisterSingleton<ImageLoaderService>(new AndroidImageLoader());
            Mvx.RegisterSingleton<IMedicineDatabaseService>(() => new DroidMedicinesDatabaseService());
            return mvxFragmentsPresenter;

        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Func<Android.App.Activity> activityResolver = () => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            UserDialogs.Init(activityResolver);
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.RegisterSingleton<INotificationService>(new AndroidNotificationService(this.ApplicationContext));
        }
    }
}
