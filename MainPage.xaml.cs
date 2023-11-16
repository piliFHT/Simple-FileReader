using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace FileModifier;

public partial class MainPage : ContentPage
{
    string path;

	public MainPage()
	{
		InitializeComponent();
	}


    private async void OpenBtn_Clicked(object sender, EventArgs e)
    {
		

		FileResult fileResult = await OpenFileDialog();
        if (fileResult != null)
        {
            path = fileResult.FullPath;
            OpenedFile.Text = $"File: {fileResult.FileName}";
            Read();
            
        }

    }

    

    private void Read()
    {
        if(File.Exists(path)) 
        {
            using (StreamReader sr = new StreamReader(path)) 
            {
                EOut.Text = sr.ReadToEnd();
            }
        }
    }

    private async Task<FileResult> OpenFileDialog()
    {
        FilePickerFileType fileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<String>>
            {
                {DevicePlatform.WinUI, new[]{".txt", ".csv", ".bat"} }
            });

        PickOptions pickOptions = new PickOptions()
        {
            PickerTitle = "Vyber textový soubor",
            FileTypes = fileType
        };

       return await FilePicker.Default.PickAsync(pickOptions);

        
    }

    private void EIn_TextChanged(object sender, TextChangedEventArgs e)
    {
        SaveBtn.BackgroundColor = Colors.Red;
        SaveBtn.TextColor = Colors.White;
    }

    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(EIn.Text);
            }
            Read();
            EIn.Text = string.Empty;
            SaveBtn.BackgroundColor = Colors.Green;
        }
        catch
        {
            await DisplayAlert("Chyba!", "Vyberte prosím prvně soubor!", "OK");
            OpenBtn_Clicked(sender, e);
        }
    }

    

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(EIn.Text);
            }
            Read();
            EIn.Text = string.Empty;
            SaveBtn.BackgroundColor = Colors.Green;
        }
        catch
        {
            await DisplayAlert("Chyba!", "Vyberte prosím prvně soubor!", "OK");
            OpenBtn_Clicked(sender, e);
        }
    }
}

