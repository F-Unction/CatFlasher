using CatFlasher.Pages.ViewModel;
using CatFlasher.Utils;
using Microsoft.Maui.Foldable;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;

namespace CatFlasher;

public partial class ImagePage : ContentPage
{
    public ImageViewModel ImageVM = new CatFlasher.Pages.ViewModel.ImageViewModel();

    public ImagePage()
    {
        InitializeComponent();
        list.BindingContext = ImageVM;
    }

    #region UglyUIProcessing
    private void AddButton_Clicked(object sender, EventArgs e)
    {
        ImageVM.Add("Unknown", "Unknown", 0x00000000, "Unknown");
    }

    private void DelButton_Clicked(object sender, EventArgs e)
    {
        ImageVM.RemoveImage(ImageVM.SelectedImage);
    }
    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        ImageVM.Images.Clear();
    }

    private void TapGestureRecognizer_RoleLabelTapped(object sender, EventArgs e)
    {
        ((Label)sender).IsVisible = false;
        ((Entry)((Grid)((Label)sender).Parent).Children[1]).IsVisible = true;
    }

    private void TapGestureRecognizer_AddressLabelTapped(object sender, EventArgs e)
    {
        ((Label)sender).IsVisible = false;
        ((Entry)((Grid)((Label)sender).Parent).Children[1]).IsVisible = true;
    }

    private void RoleEntry_Completed(object sender, EventArgs e)
    {
        ((Entry)sender).IsVisible = false;
        ((Label)((Grid)((Entry)sender).Parent).Children[0]).IsVisible = true;
    }

    private void AddressEntry_Completed(object sender, EventArgs e)
    {
        ((Entry)sender).IsVisible = false;
        ((Label)((Grid)((Entry)sender).Parent).Children[0]).IsVisible = true;
    }

    private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        ImageVM.SelectedImage = (Bootloader.Image)e.SelectedItem;
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var option = new PickOptions();
        option.FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".img" } }
            });

        var result = await FilePicker.Default.PickAsync(option);
        if (result != null)
        {
            ((Label)sender).Text = result.FullPath;

            if (result.FullPath.EndsWith("xloader.img"))
            {
                ((Entry)((Grid)((Grid)((Label)sender).Parent).Children[1]).Children[1]).Text = "xloader";
                ((Entry)((Grid)((Grid)((Label)sender).Parent).Children[2]).Children[1]).Text = "0x00020000";
            }
            else if (result.FullPath.EndsWith("fastboot.img"))
            {
                ((Entry)((Grid)((Grid)((Label)sender).Parent).Children[1]).Children[1]).Text = "fastboot";
                ((Entry)((Grid)((Grid)((Label)sender).Parent).Children[2]).Children[1]).Text = "0x10000000";
            }

            try
            {
                using (FileStream fop = System.IO.File.OpenRead(result.FullPath))
                {
                    if (fop.Length < 10000000)//10 mb
                        ((Label)((Grid)((Label)sender).Parent).Children[3]).Text = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(fop)).Replace("-", "").ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        else
        {
            ((Label)sender).Text = "Unknown";
        }
    }
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var appShell = (AppShell)Application.Current.MainPage;
        var devicePage = ((DevicePage)
            (appShell.imagePage.GetPrivate<IEnumerable<Microsoft.Maui.Controls.Element>>("AllChildren")
            .First()));
        var devicePicker = devicePage.devicePicker;
        //var devicePicker = ((DevicePage)appShell.devicePage.Content).devicePicker;

        if (devicePicker.SelectedIndex == -1)
        {
            DisplayAlert("Error!", "No connected devices!", "OK");
            return;
        }
        //IsEnabled = false;
        var strImgList = "";
        foreach (Bootloader.Image i in ImageVM.Images)
        {
            strImgList += i.Path + " as " + i.Role + " to " + (string)(new HexToDecConverter().Convert(i.Address, null, null, null));
        }
        if (!DisplayAlert("Sure to flash this?", strImgList, "Yes", "No").Result)
        {
            return;
        }

        var eventArgs = new FlashArgs
        {
            TargetMode = devicePage.IsSelectedDeviceInFastbootMode
                ? UsbController.Device.DMode.Fastboot
                : UsbController.Device.DMode.DownloadVCOM,
            Target = devicePicker.SelectedItem.ToString(),
            //DisableFBLOCK = disableFBLOCK.IsChecked.Value,
            //Reboot = reboot.IsChecked.Value
        };

        if (!devicePage.IsSelectedDeviceInFastbootMode)
        {
            eventArgs.Bootloader = new Bootloader("", "", ImageVM.Images.ToArray());
            //bootloaders.First(x => x.Title == deviceBootloader.SelectedItem.ToString());
        }

        ((FastbootPage)
            (appShell.fastbootPage.GetPrivate<IEnumerable<Microsoft.Maui.Controls.Element>>("AllChildren")
            .First())).core.StartProcess(eventArgs);
    }
}