using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.PictureChooser;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Piller.ViewModels
{
    public class PhotoGalleryViewModel : MvxViewModel
    {
        //picture
        IMvxPictureChooserTask PictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();
        public ReactiveCommand<Unit, Stream> TakePhotoCommand { get; set; }
        public ReactiveCommand<Unit, Stream> SelectPhotoCommand { get; set; }

        //picture
        private byte[] bytes;
        public byte[] Bytes
        {
            get { return bytes; }
            set { this.SetProperty(ref bytes, value); }
        }

        //picture
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
            this.TakePhotoCommand.Subscribe(x => {
                this.OnPicture(x);
            });
            this.SelectPhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.ChoosePictureFromLibrary(400, 75));
            this.SelectPhotoCommand.Subscribe(x => this.OnPictureSelect(x));

        }
    }
}
