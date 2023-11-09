using IronBarCode;

namespace MAUI_Barcode
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SelectBarcode(object sender, EventArgs e)
        {
            try
            {
                var images = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Pick image",
                    FileTypes = FilePickerFileType.Images
                });

                if (images == null) // In case user cancels the file picker
                {
                    await DisplayAlert("Alert", "Please select an image to proceed further!", "OK");
                    return;
                }

                var imageSource = images.FullPath;
                barcodeImage.Source = imageSource;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void ScanBarcode(object sender, EventArgs e)
        {
            try
            {
                var imageSource = (FileImageSource)barcodeImage.Source;
                if (imageSource == null)
                {
                    await DisplayAlert("Alert", "Please select an image first!", "OK");
                    return;
                }

                var barcodeResults = BarcodeReader.Read(imageSource.File);
                if (barcodeResults != null && barcodeResults.Any())
                {
                    var result = barcodeResults.First().Text;
                    outputText.Text = result;
                }
                else
                {
                    await DisplayAlert("Alert", "No barcode detected!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void CopyEditorText(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(outputText.Text))
                {
                    await DisplayAlert("Error", "There's no text to copy!", "OK");
                    return;
                }

                await Clipboard.SetTextAsync(outputText.Text);
                await DisplayAlert("Success", "Text is copied!", "OK");
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error alert.
                await DisplayAlert("Error", $"Failed to copy text: {ex.Message}", "OK");
            }
        }

    }
}
