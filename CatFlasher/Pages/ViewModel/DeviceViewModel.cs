using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFlasher.Pages.ViewModel
{
    public partial class DeviceViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> devices;

        public DeviceViewModel()
        {
            devices = new ObservableCollection<string>();
        }

        public void Add(string item)
        {
            devices.Add(item);
            OnPropertyChanged(nameof(Devices));
        }

        public void Clear()
        {
            devices.Clear();
            OnPropertyChanged(nameof(Devices));
        }
    }
}
