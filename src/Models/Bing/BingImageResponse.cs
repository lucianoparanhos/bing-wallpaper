using System.Collections.Generic;

namespace UWP_Bing_Wallpaper.Models.Bing
{
    public class BingImageResponse
    {
        public IEnumerable<BingImage> Images { get; set; }
    }
}