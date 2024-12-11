using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavigationWinUI3Example.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string AppName { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            AppName = ((App)Application.Current).AppName;
        }

        private void NavigationView_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            var selectedItem = (NavigationViewItem)e.SelectedItem;
            if (e.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Settings), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            else if (selectedItem != null && selectedItem.Tag.ToString() == "Extend")
            {
                    var newWindow = WindowHelper.CreateWindow();
                    var rootPage = new Frame();
                    newWindow.Content = rootPage;
                    newWindow.Activate();

                    // C# code to navigate in the new window
                    var targetPageType = typeof(MainPage);
                    string targetPageArguments = string.Empty;
                    rootPage.Navigate(targetPageType, targetPageArguments);
            }
            else if(selectedItem != null)
            {
                string pageName = selectedItem.Tag.ToString();
                Type pageType = Type.GetType(pageName);

                if (pageType != null)
                {
                    //Frame.Navigate(pageType);
                    contentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                }
                else
                {
                    ShowDialog_Click(this, null);
                }
            }        
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();
            }
        }

        private async void ShowDialog_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "404 Error: Page not found.";
            dialog.PrimaryButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "A page could not be instatiated from the Navigation View item tag.";

            var result = await dialog.ShowAsync();
        }

    }
}
