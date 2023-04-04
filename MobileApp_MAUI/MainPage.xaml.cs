using MauiMedia.Classes;
using Microsoft.Maui.Graphics.Platform;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace MauiMedia;

public partial class MainPage : ContentPage
{
    private int selectedCompressionQuality;

    public MainPage()
    {
        InitializeComponent();        
    }

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        await ProcessPhotoAsync(false);
    }
    private async Task ProcessPhotoAsync(bool useCamera)
    {
        //await CheckAndRequestLocationPermission();
        //var photo = useCamera
        //  ? await MediaPicker.Default.CapturePhotoAsync()
        //  : await MediaPicker.Default.PickPhotoAsync();


        //if (photo is { })
        //{
            UploadedOrSelectedImage.Source = "test_image.png";
        FileSizeLabel.Text = "Either it is not a flower or it is not on the prediction list.";
        //// Resize to allowed size - 4MB
        //var resizedPhoto = await ImagePrediction.ResizePhotoStream(photo);
        //var result1 = ImagePrediction.MakePredictionRequest(resizedPhoto).Result;

        //if(result1.IsFlower)
        //{
        //    FileSizeLabel.Text = $"It's a{result1.TagName.ToString().ToUpper()}";
        //}
        //else
        //{
        //    FileSizeLabel.Text = "Either it is not a flower or it is not on the prediction list.";
        //}

        //}
        //else
        //{
        //    FileSizeLabel.Text = "";
        //}
    }


    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        await ProcessPhotoAsync(true);
    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        selectedCompressionQuality = (int)((sender as Slider).Value * 100);
        PredictionLabel.Text = $"Prediction: {selectedCompressionQuality}";
    }


}

