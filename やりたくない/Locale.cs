using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace やりたくない
{
    internal static class Locale
    {
        public static event yaritakunaiEventHandler onLocaleLoadOrChange = delegate { };
        private static Dictionary<string, string> current_locale_keys;
        public static void switchLocaleTo(Languages to)
        {
            Settings.language = to;
            string localefilename = "";
            switch (Settings.language)
            {
                case Languages.日本語:
                    localefilename = "jp";
                    break;
                case Languages.English:
                    localefilename = "en";
                    break;
                default:
                    localefilename = "jp";
                    break;
            }
            //load locale's json file
            string embedName = "やりたくない.Resources." + localefilename + "_tr.json";
            string allJson = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream(embedName)).ReadToEnd();
            current_locale_keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(allJson);
            onLocaleLoadOrChange();
        }
        /// <summary>
        /// Gets a translation for a keyword.  Please only use in initialization of text, because it is (relatively) slow.
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static string getTRFromKey(string keyWord)
        {
            try
            {
                return current_locale_keys[keyWord];
            }
            catch
            {
#if DEBUG
                throw new Exception($"Could not find translation for key: {keyWord}. Either add a temp key to the language you're using, or if you know it, add a translation. Language: {Settings.language.ToString()}");

#else
                throw new Exception($"Could not find translation for key: {keyWord}. Please contact the developer with this information.");

#endif

            }
        }
    }
}
