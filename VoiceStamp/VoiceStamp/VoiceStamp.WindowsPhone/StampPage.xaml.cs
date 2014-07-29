using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace VoiceStamp
{
   public sealed partial class StampPage : Page
   {
      private MediaCapture _mediaCapture;
      private InMemoryRandomAccessStream _audioStream;
      private bool _isRecording;

      public StampPage()
      {
         InitializeComponent();
      }

      private async void Page_Loaded( object sender, Windows.UI.Xaml.RoutedEventArgs e )
      {
         await InitMediaCapture();
      }

      private async Task InitMediaCapture()
      {
         _mediaCapture = new MediaCapture();

         var captureInitSettings = new MediaCaptureInitializationSettings();
         captureInitSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;
         //captureInitSettings.MediaCategory = MediaCategory.Other;

         await _mediaCapture.InitializeAsync( captureInitSettings );

         _mediaCapture.Failed += MediaCaptureOnFailed;
         _mediaCapture.RecordLimitationExceeded += MediaCaptureOnRecordLimitationExceeded;
      }

      private void MediaCaptureOnRecordLimitationExceeded( MediaCapture sender )
      {
      }

      private void MediaCaptureOnFailed( MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs )
      {
      }

      private async Task StartRecording()
      {
         var mediaEncodingProfile = MediaEncodingProfile.CreateM4a( AudioEncodingQuality.Auto );

         _audioStream = new InMemoryRandomAccessStream();

         await _mediaCapture.StartRecordToStreamAsync( mediaEncodingProfile, _audioStream );
      }

      private async Task StopRecording()
      {
         await _mediaCapture.StopRecordAsync();
      }

      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
         var bitmapImage = (BitmapImage) e.Parameter;

         ImagePreview.Source = bitmapImage;
      }

      private async void Image_PointerPressed( object sender, PointerRoutedEventArgs e )
      {
         if ( _isRecording )
         {
            return;
         }

         _isRecording = true;

         const int stampSize = 70;
         var pointerPoint = e.GetCurrentPoint( ImagePreview );

         var rectangle = new Rectangle
         {
            Width = stampSize,
            Height = stampSize,
            Fill = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) )
         };

         Canvas.SetLeft( rectangle, pointerPoint.Position.X - stampSize / 2 );
         Canvas.SetTop( rectangle, pointerPoint.Position.Y - stampSize / 2 );

         PageCanvas.Children.Add( rectangle );

         await StartRecording();
      }

      private async void Image_PointerReleased( object sender, PointerRoutedEventArgs e )
      {
         _isRecording = false;

         StopRecording();

         MediaElement.SetSource( _audioStream, "audio/m4a" );

         MediaElement.Play();

         //string fileName = DateTime.Now.ToFileTime().ToString();

         //var storageFile = await KnownFolders.VideosLibrary.CreateFileAsync( fileName );

         //using ( var dataReader = new DataReader( _audioStream.GetInputStreamAt( 0 ) ) )
         //{
         //   await dataReader.LoadAsync( (uint) _audioStream.Size );

         //   byte[] buffer = new byte[(int) _audioStream.Size];

         //   dataReader.ReadBytes( buffer );

         //   await FileIO.WriteBytesAsync( storageFile, buffer );
         //}
      }
   }
}
