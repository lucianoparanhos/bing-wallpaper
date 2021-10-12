using System.Collections.Generic;

namespace UWP_Bing_Wallpaper.Models.Bing
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/previous-versions/bing/search/dd251064(v=msdn.10)?redirectedfrom=MSDN
    /// </summary>
    public static class BingLocales
    {
        public static IEnumerable<string> Locales { get; private set; } = new List<string>
        {
            "ar-XA", "bg-BG", "cs-CZ", "da-DK", "de-AT", "de-CH", "de-DE", "el-GR",
            "en-AU", "en-CA", "en-GB", "en-ID", "en-IE", "en-IN", "en-MY", "en-NZ",
            "en-PH", "en-SG", "en-US", "en-XA", "en-ZA", "es-AR", "es-CL", "es-ES",
            "es-MX", "es-US", "es-XL", "et-EE", "fi-FI", "fr-BE", "fr-CA", "fr-CH",
            "fr-FR", "he-IL", "hr-HR", "hu-HU", "it-IT", "ja-JP", "ko-KR", "lt-LT",
            "lv-LV", "nb-NO", "nl-BE", "nl-NL", "pl-PL", "pt-BR", "pt-PT", "ro-RO",
            "ru-RU", "sk-SK", "sl-SL", "sv-SE", "th-TH", "tr-TR", "uk-UA", "zh-CN",
            "zh-HK", "zh-TW",
        };

        public static string DefaultLocale => "en-US";
    }
}