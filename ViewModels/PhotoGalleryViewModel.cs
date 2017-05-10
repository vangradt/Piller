using System;
using MvvmCross.Plugins.PictureChooser;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using ReactiveUI;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.IO;
using Piller.Services;
using Piller.Data;
using Android.Util;
using MvvmCross.Plugins.Messenger;
using RxUI = ReactiveUI;

namespace Piller.ViewModels
{
    public class PhotoGalleryViewModel : MvxViewModel
    {
        IMvxPictureChooserTask PictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();
        public ReactiveCommand<Unit, Stream> TakePhotoCommand { get; set; }
        public ReactiveCommand<Unit, Stream> SelectPhotoCommand { get; set; }
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        Data.MedicationDosage item;

        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }

        private byte[] _bytes;
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; RaisePropertyChanged(() => Bytes); }
        }

        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
        }

        private void OnPictureSelect(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
        }

        public PhotoGalleryViewModel()
        {
            this.TakePhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.TakePicture(400, 75));
            this.TakePhotoCommand.Subscribe(x =>
            {
                this.OnPicture(x);
            });
            this.SelectPhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.ChoosePictureFromLibrary(400, 75));
            this.SelectPhotoCommand.Subscribe(x => this.OnPictureSelect(x));
            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool>(async _ =>
            {
                var dataRecord = new MedicationDosage
                {
                    Id = item.Id,
                    Name = item.Name,
                    Dosage = item.Dosage,
                    Days = item.Days,
                    DosageHours = item.DosageHours,
                    Bytes = this.Bytes
                };
                await this.storage.SaveAsync<MedicationDosage>(dataRecord);

                return true;
            });
            this.Save
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
            item = await storage.GetAsync<Data.MedicationDosage>(nav.MedicationDosageId);
            Log.Debug("id", item.Id.ToString());
            this.Bytes = item.Bytes;
        }
    }
}   

