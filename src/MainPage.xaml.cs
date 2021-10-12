using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UWP_Bing_Wallpaper.Models.Bing;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP_Bing_Wallpaper
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Hide default title bar.
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            LoadLocaleComboBox();
            LoadBingImageData();
        }

        #region ComboBox

        private readonly ObservableCollection<CultureInfo> BingCultures = new ObservableCollection<CultureInfo>();
        private CultureInfo selectedCultureInfo;

        private void LoadLocaleComboBox()
        {
            selectedCultureInfo = CultureInfo.CurrentCulture ?? CultureInfo.GetCultureInfo(BingLocales.DefaultLocale);

            if (CultureInfo.CurrentCulture == null)
            {
                BingCultures.Add(selectedCultureInfo);
            }
            else
            {
                BingLocales.Locales.ToList().ForEach(culture => BingCultures.Add(CultureInfo.GetCultureInfo(culture)));
            }
        }

        private void LocaleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox cb) || cb.IsLoaded == false || !(cb.SelectedItem is CultureInfo cultureInfo))
            {
                return;
            }

            if (cultureInfo.Name != selectedCultureInfo.Name)
            {
                selectedCultureInfo = cultureInfo;
                LoadBingImageData();
            }
        }

        #endregion ComboBox

        private BingImageResponse _bingImageResponse;

        private async void LoadBingImageData()
        {
            _bingImageResponse = await GetData(selectedCultureInfo);
            LoadImages();
        }

        private void LoadImages()
        {
            if (_bingImageResponse == null)
            {
                // TODO: Load default image
                return;
            };

            List<Image> imageList = GenerateImageList(_bingImageResponse.Images);

            Gallery.ItemsSource = imageList;
            FlipViewPipsPage.NumberOfPages = imageList.Count;

            Gallery.SelectionChanged += FlipView_SelectionChanged;
            Gallery.SelectedIndex = imageList.Count - 1;
        }

        private List<Image> GenerateImageList(IEnumerable<BingImage> images)
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

            BingImage selectedImage = _bingImageResponse.Images.SingleOrDefault(x => x.Hsh == selectedItemName);

            //NavigationViewControl.Header = selectedImage.Title;
            BingHyperLink.Content = selectedImage.Copyright;
            BingHyperLink.NavigateUri = new Uri(selectedImage.CopyrightLink);

            BingCopyright.Text = selectedImage.Copyright;
        }

        private async Task<BingImageResponse> GetData(CultureInfo cultureInfo)
        {
            int idx = 0;
            int n = 8;
            string mkt = cultureInfo.Name;

            var requestUri = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx={0}&n={1}&mkt={2}", idx, n, mkt);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetFromJsonAsync<BingImageResponse>(requestUri);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}