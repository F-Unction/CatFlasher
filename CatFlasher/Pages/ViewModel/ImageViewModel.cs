using CatFlasher.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatFlasher.Pages.ViewModel
{
    public class ImageViewModel : INotifyPropertyChanged
    {
        Bootloader.Image selectedImage;

        public ObservableCollection<Bootloader.Image> Images
        {
            get; set;
        }

        public Bootloader.Image SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                if (selectedImage != value)
                {
                    selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        public ICommand DeleteCommand => new Command<Bootloader.Image>(RemoveImage);

        public ImageViewModel()
        {
            Images= new ObservableCollection<Bootloader.Image>(new List<Bootloader.Image>());
            SelectedImage = Images.FirstOrDefault();
            OnPropertyChanged(nameof(SelectedImage));
        }

        public void Add(string path,string role,uint address, string hash=null)
        {
            Images.Add(new Bootloader.Image(path, role, address, hash));
            OnPropertyChanged(nameof(Images));
        }

        public void RemoveImage(Bootloader.Image monkey)
        {
            if (Images.Contains(monkey))
            {
                Images.Remove(monkey);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
