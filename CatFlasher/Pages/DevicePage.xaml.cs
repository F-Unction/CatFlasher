using CatFlasher.Utils;
using CatFlasher.WinUI;

namespace CatFlasher;

public partial class DevicePage : ContentPage
{
    public class ShowingDevice
    {
        public string Name { get; set; }

        public ShowingDevice(string name)
        {
            Name = name;
        }
    }

    private UsbController usbController;
    //public UsbController.Device[] devices;

    public bool IsSelectedDeviceInFastbootMode = false;
    public List<ShowingDevice> showingDevices= new List<ShowingDevice>();

    public DevicePage()
	{
        InitializeComponent();

        //fuckPicker.BindingContext = this;
        //fuckPicker.SetBinding(Picker.ItemsSourceProperty, "showingDevices");
        //fuckPicker.ItemDisplayBinding = new Binding("Name");

        usbController = new UsbController();
        usbController.Notify += RefreshDevices;
        usbController.StartWorker();

        //showingDevices.Add(new ShowingDevice("fuckyoutoo"));

    }

    private void RefreshDevices(UsbController.Device[] devices)
    {
        if (Dispatcher.IsDispatchRequired)
        {
            Dispatcher.Dispatch(() => RefreshDevices(devices));
            return;
        }

        devicePicker.Items.Clear();

        
        foreach ( var device in devices)
        {
            devicePicker.Items.Add(device.Mode == UsbController.Device.DMode.DownloadVCOM
                ? device.Description
                : $"Fastboot: {device.Description}");
        }

        if (devicePicker.SelectedIndex == -1 && devices.Length > 0)
        {
            devicePicker.SelectedIndex = 0;
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        showingDevices.Add(new ShowingDevice("fuck you too"));
        
        //RefreshDevices();

        /*
        var appShell = (AppShell)Application.Current.MainPage;
        appShell.CurrentItem = appShell.imagePage;
        */
    }

    private void devicePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (devicePicker.SelectedIndex == -1)
        {
            IsSelectedDeviceInFastbootMode = false;
        }
        else
        {
            IsSelectedDeviceInFastbootMode = devicePicker.SelectedItem.ToString().StartsWith("Fastboot");
        }
    }
}