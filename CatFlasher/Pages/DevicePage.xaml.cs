using CatFlasher.Pages.ViewModel;
using CatFlasher.Utils;
using CatFlasher.WinUI;

namespace CatFlasher;

public partial class DevicePage : ContentPage
{

    private UsbController usbController;
    //public UsbController.Device[] devices;

    public bool IsSelectedDeviceInFastbootMode = false;
    public DeviceViewModel viewModel = new DeviceViewModel();

    public DevicePage()
	{
        InitializeComponent();

        this.BindingContext = viewModel;

        usbController = new UsbController();
        usbController.Notify += RefreshDevices;
        usbController.StartWorker();
    }

    private void RefreshDevices(UsbController.Device[] devices)
    {
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() => RefreshDevices(devices));
            return;
        }

        viewModel.Clear();
        
        foreach ( var device in devices)
        {
            viewModel.Add(device.Mode == UsbController.Device.DMode.DownloadVCOM
                ? device.Description
                : $"Fastboot: {device.Description}");
        }

        if (picker.SelectedIndex == -1 && devices.Length > 0)
        {
            picker.SelectedIndex = 0;
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var appShell = (AppShell)Application.Current.MainPage;
        appShell.CurrentItem = appShell.imagePage;
    }

    private void devicePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (picker.SelectedIndex == -1)
        {
            IsSelectedDeviceInFastbootMode = false;
        }
        else
        {
            IsSelectedDeviceInFastbootMode = picker.SelectedItem.ToString().StartsWith("Fastboot");
        }
    }
}