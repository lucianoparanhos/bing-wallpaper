using System.Collections.ObjectModel;

namespace UWP_Bing_Wallpaper.Models.Bing
{
    public class BingWallpaperResolution
    {
        public uint ScreenWidth { get; }
        public uint ScreenHeight { get; }

        public BingWallpaperResolution(uint screenWidth, uint screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }

        public override string ToString()
        {
            var result = string.Format("{0} x {1}", ScreenWidth.ToString(), ScreenHeight.ToString());

            if (ScreenWidth > 1920)
            {
                result = "UHD";
            }

            return result;
        }

        //private IEnumerable<string> Resolutions => new List<string>
        //{
        //    "800x600", "1024x768", "1280x720", "1280x768", "1366x768", "1920x1080", "1920x1200", "UHD"
        //};

        internal ObservableCollection<BingWallpaperResolution> GetResolutions()
        {
            var result = new ObservableCollection<BingWallpaperResolution>
            {
                new BingWallpaperResolution(800, 600),
                new BingWallpaperResolution(1024, 768),
                new BingWallpaperResolution(1280, 720),
                new BingWallpaperResolution(1280, 768),
                new BingWallpaperResolution(1366, 768),
                new BingWallpaperResolution(1920, 1080),
                new BingWallpaperResolution(1920, 1200),
                new BingWallpaperResolution(ScreenWidth, ScreenHeight)
            };

            return result;
        }
    }
}