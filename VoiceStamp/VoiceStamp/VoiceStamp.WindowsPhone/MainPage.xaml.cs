using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VoiceStamp
{
   public sealed partial class MainPage : Page
   {
      public MainPage()
      {
         InitializeComponent();

         NavigationCacheMode = NavigationCacheMode.Required;
      }

      protected override void OnNavigatedTo( NavigationEventArgs e )
      {
      }
   }
}
