using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VoiceStamp
{
   public sealed partial class MainPage : Page, IFileOpenPickerContinuable
   {
      public MainPage()
      {
         InitializeComponent();

         NavigationCacheMode = NavigationCacheMode.Required;
      }

      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
      }

      private void ImportButton_Click( object sender, RoutedEventArgs e )
      {
         FileOpenPicker openPicker = new FileOpenPicker();

         openPicker.ViewMode = PickerViewMode.Thumbnail;

         openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

         openPicker.FileTypeFilter.Add( ".jpg" );
         openPicker.FileTypeFilter.Add( ".jpeg" );
         openPicker.FileTypeFilter.Add( ".png" );

         openPicker.ContinuationData["Operation"] = "ImportedPicture";

         openPicker.PickSingleFileAndContinue();
      }

      private void CameraButton_Click( object sender, RoutedEventArgs e )
      {
         //Frame.Navigate( typeof( CameraPage ) );
      }

      public async void ContinueFileOpenPicker( FileOpenPickerContinuationEventArgs args )
      {
         if ( (args.ContinuationData["Operation"] as string) == "ImportedPicture" && args.Files.Count > 0 )
         {
            StorageFile file = args.Files[0];

            IRandomAccessStream fileStream = await file.OpenAsync( FileAccessMode.Read );
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.SetSource( fileStream );

            Frame.Navigate( typeof( StampPage ), bitmapImage );
         }
      }
   }
}
