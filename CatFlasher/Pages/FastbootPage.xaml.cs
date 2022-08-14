using CatFlasher.Utils;
using System.Text;

namespace CatFlasher;

public partial class FastbootPage : ContentPage
{
    public Core core = new Core();
    public FastbootPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            core.fb.Connect();

            var response = core.fb.Command(Encoding.ASCII.GetBytes(entry.Text));
            await DisplayAlert("Command response", Encoding.ASCII.GetString(response.RawData), "OK");

            core.fb.Disconnect();
        }
        catch (Exception ex)
        {
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }