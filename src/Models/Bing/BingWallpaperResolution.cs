using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UWP_Bing_Wallpaper.Models.Bing
{
    public class BingWallpaperResolution
    {
        public uint ScreenWidth { get; }
        public uint ScreenHeight { get; }

        public bool IsRecommended { get; private set; }

        public BingWallpaperResolution(uint screenWidth, uint screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }

        private BingWallpaperResolution(uint screenWidth, uint screenHeight, uint actualScreenWidth, uint actualScreenHeight)
        {
        }

        public override string ToString()
        {
            string result = ScreenWidth > 1920 ? "UHD" : string.Format("{0} x {1}", ScreenWidth.ToString(), ScreenHeight.ToString());

            if (IsRecommended == true)
            {
                result += " (Recommended)";
            }

            return result;
        }

        internal ObservableCollection<BingWallpaperResolution> GetResolutions()
        {
            List<BingWallpaperResolution> resolutions = new List<BingWallpaperResolution> {
                new BingWallpaperResolution(800, 600),
                new BingWallpaperResolution(1024, 768),
                new BingWallpaperResolution(1280, 720),
                new BingWallpaperResolution(1280, 768),
                new BingWallpaperResolution(1366, 768),
                new BingWallpaperResolution(1920, 1080),
                new BingWallpaperResolution(1920, 1200),
                new BingWallpaperResolution(1921, 1201)
            };

            var sortedList = resolutions
                .OrderByDescending(x => x.ScreenWidth)
                .ThenByDescending(x => x.ScreenHeight)
                .ToList();

            bool stop = false;
            sortedList.ForEach(x =>
            {
                if (stop) return;
                stop = x.IsRecommended = x.ScreenWidth <= ScreenWidth && x.ScreenHeight <= ScreenHeight;
            });

            return new ObservableCollection<BingWallpaperResolution>(sortedList);
        }
    }
}