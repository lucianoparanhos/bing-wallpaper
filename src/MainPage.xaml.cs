using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UWP_Bing_Wallpaper.Models;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_Bing_Wallpaper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            //Register a handler for when the window changes focus
            Window.Current.Activated += Current_Activated;

            GetBingData();
        }

        private BingResponse _bingResponse;

        private async void GetBingData()
        {
            _bingResponse = await GetData();

            LoadImages();
        }


        private void LoadImages()
        {
            if (_bingResponse == null) {
                // TODO: Load default image
                return;
            };

            List<Image> imageList = GenerateImageList(_bingResponse.Images);

            Gallery.ItemsSource = imageList;
            FlipViewPipsPage.NumberOfPages = imageList.Count;

            Gallery.SelectionChanged += FlipView_SelectionChanged;
            Gallery.SelectedIndex = imageList.Count - 1;

        }

        private List<Image> GenerateImageList(IList<BingImage> images)
        {
            if (images == null || images.Any() == false)
            {
                return null;
            }

            List<Image> result = new List<Image>();

            images.OrderBy(x => x.StartDate).ToList().ForEach(img =>
            {
                result.Add(new Image
                {
                    Source = new BitmapImage(new Uri(img.UrlFull)),
                    Stretch = Stretch.Fill,
                    Name = img.Hsh
                });
            });

            return result;
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is FlipView fv) || !(fv.SelectedItem is Image img))
            {
                return;
            }


            string selectedItemName = img.Name;

            BingImage selectedImage = _bingResponse.Images.SingleOrDefault(x => x.Hsh == selectedItemName);

            NavigationViewControl.Header = selectedImage.Copyright;
        }

        private async Task<BingResponse> GetData()
        {
            var idx = 0;
            var n = 8;
            var mkt = "pt-BR";

            var requestUri = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx={0}&n={1}&mkt={2}", idx, n, mkt);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetFromJsonAsync<BingResponse>(requestUri);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Update title bar control size as needed to account for system size changes.
            AppTitleBar.Height = coreTitleBar.Height;

            // Ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        // Update the TitleBar based on the inactive/active state of the app
        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
            SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];

            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                AppTitle.Foreground = inactiveForegroundBrush;
            }
            else
            {
                AppTitle.Foreground = defaultForegroundBrush;
            }
        }

        // Update the TitleBar content layout depending on NavigationView DisplayMode
        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            const int topIndent = 16;
            const int expandedIndent = 48;
            int minimalIndent = 104;

            // If the back button is not visible, reduce the TitleBar content indent.
            if (NavigationViewControl.IsBackButtonVisible.Equals(Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible.Collapsed))
            {
                minimalIndent = 48;
            }

            Thickness currMargin = AppTitleBar.Margin;

            // Set the TitleBar margin dependent on NavigationView display mode
            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                AppTitleBar.Margin = new Thickness(topIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness(minimalIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else
            {
                AppTitleBar.Margin = new Thickness(expandedIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
        }
    }
}