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
using MvvmCross.Plugins.Messenger;
using RxUI = ReactiveUI;

namespace Piller.ViewModels
{
    public class PhotoGalleryViewModel : MvxViewModel
    {
        IMvxPictureChooserTask PictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();
        public ReactiveCommand<Unit, Stream> TakePhotoCommand { get; set; }
        public ReactiveCommand<Unit, Stream> SelectPhotoCommand { get; set; }
        public ReactiveCommand<Unit, Stream> TakePhotoPillCommand { get; set; }
        public ReactiveCommand<Unit, Stream> SelectPhotoPillCommand { get; set; }
        public ReactiveCommand<Unit, Stream> TakePhotoRxCommand { get; set; }
        public ReactiveCommand<Unit, Stream> SelectPhotoRxCommand { get; set; }

        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        Data.MedicationDosage item;

        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }

        private byte[] _bytes;
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; RaisePropertyChanged(() => Bytes); }
        }

        private byte[] _bytes_pill;
        public byte[] Bytes_pill
        {
            get { return _bytes_pill; }
            set { _bytes_pill = value; RaisePropertyChanged(() => Bytes_pill); }
        }

        private byte[] _bytes_rx;
        public byte[] Bytes_rx
        {
            get { return _bytes_rx; }
            set { _bytes_rx = value; RaisePropertyChanged(() => Bytes_rx); }
        }

        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
        }
        private void OnPicturePill(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes_pill = memoryStream.ToArray();
        }
        private void OnPictureRx(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes_rx = memoryStream.ToArray();
        }

        public PhotoGalleryViewModel()
        {
            this.TakePhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.TakePicture(400, 75));
            this.TakePhotoCommand.Subscribe(x => this.OnPicture(x));
            this.TakePhotoPillCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.TakePicture(400, 75));
            this.TakePhotoPillCommand.Subscribe(x => this.OnPicturePill(x));
            this.TakePhotoRxCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.TakePicture(400, 75));
            this.TakePhotoRxCommand.Subscribe(x => this.OnPictureRx(x));

            this.SelectPhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.ChoosePictureFromLibrary(400, 75));
            this.SelectPhotoCommand.Subscribe(x => this.OnPicture(x));
            this.SelectPhotoPillCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.ChoosePictureFromLibrary(400, 75));
            this.SelectPhotoPillCommand.Subscribe(x => this.OnPicturePill(x));
            this.SelectPhotoRxCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.ChoosePictureFromLibrary(400, 75));
            this.SelectPhotoRxCommand.Subscribe(x => this.OnPictureRx(x));

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool>(async _ =>
            {
                var dataRecord = new MedicationDosage
                {
                    Id = item.Id,
                    Name = item.Name,
                    Dosage = item.Dosage,
                    Days = item.Days,
                    DosageHours = item.DosageHours,
                    Bytes = this.Bytes,
                    Bytes_pill = this.Bytes_pill,
                    Bytes_rx = this.Bytes_rx
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
            this.Bytes = item.Bytes;
            this.Bytes_pill = item.Bytes_pill;
            this.Bytes_rx = item.Bytes_rx;
        }
    }
}   

