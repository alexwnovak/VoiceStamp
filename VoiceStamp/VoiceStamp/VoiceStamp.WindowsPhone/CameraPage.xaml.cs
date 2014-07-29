using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VoiceStamp
{
   public sealed partial class CameraPage : Page
   {
      private MediaCapture _mediaCapture;

      public CameraPage()
      {
         InitializeComponent();
      }

      private async void OnLoaded( object sender, RoutedEventArgs e )
      {
         await InitializeCamera();

         await StartPreview();
      }

      private async Task InitializeCamera()
      {
         _mediaCapture = new MediaCapture();

         await _mediaCapture.InitializeAsync();
      }

      private async Task StartPreview()
      {
         CapturePreview.Source = _mediaCapture;

         await _mediaCapture.StartPreviewAsync();
      }
   }
}
