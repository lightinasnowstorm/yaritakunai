using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;
using Microsoft.CSharp;

namespace やりたくない
{
    enum Languages
    {
        日本語,
        English
    }

    internal static class Settings
    {
        public static string savesLocation => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "My Games" + Path.DirectorySeparatorChar + "やりたくない" + Path.DirectorySeparatorChar;
        public static string playerSavesLocation => savesLocation + Path.DirectorySeparatorChar + "Players" + Path.DirectorySeparatorChar;
        public static string worldsSavesLocation => savesLocation + Path.DirectorySeparatorChar + "Worlds" + Path.DirectorySeparatorChar;
        public static Formatting formatting;
        public static Color cursorColour;
        public static Color pseudoCursorColour;
        public static Languages language;
        public static Keys IMEKey;
        public static bool init()
        {
            return load();
        }

        public static bool load()
        {
            try
            {
                dynamic jsonSettings = JsonConvert.DeserializeObject(File.ReadAllText(savesLocation + "settings.json"));
                Locale.switchLocaleTo((Languages)Enum.Parse(typeof(Languages), jsonSettings.language.Value));
                dynamic loadedCursorColour = jsonSettings.cursorColour;
                cursorColour = new Color((int)loadedCursorColour.R, (int)loadedCursorColour.G, (int)loadedCursorColour.B, (int)loadedCursorColour.A);
                dynamic loadedPseudoCursorColour = jsonSettings.pseudoCursorColour;
                pseudoCursorColour = new Color((int)loadedPseudoCursorColour.R, (int)loadedPseudoCursorColour.G, (int)loadedPseudoCursorColour.B, (int)loadedPseudoCursorColour.A);
                IMEKey = (Keys)Enum.Parse(typeof(Keys), jsonSettings.IMEKey.Value);
                formatting = (Formatting)Enum.Parse(typeof(Formatting), jsonSettings.formatting.Value);
                //loaded settings
                return true;
            }
            catch
            {
                defaults();
                return false;
            }
        }

        public static void defaults()
        {
            Locale.switchLocaleTo(Languages.日本語);
            cursorColour = Color.Red;
            pseudoCursorColour = Color.Aqua;
            IMEKey = Keys.OemTilde;
            formatting = Formatting.Indented;
        }

        public static void save()
        {
            var saveData = new { cursorColour, pseudoCursorColour, language = language.ToString(), IMEKey = IMEKey.ToString(), formatting = formatting.ToString() };
            File.WriteAllText(savesLocation + "settings.json", JsonConvert.SerializeObject(saveData, formatting));
        }

        public static void makeSureSettingsDirectoriesExist()
        {
            if (!directoryAssert(savesLocation))
            {
                throw new Exception(String.Format(Locale.getTRFromKey("Settings::DirectoryFailed"), Locale.getTRFromKey("Directories::Saves")));
            }
            if (!directoryAssert(playerSavesLocation))
            {
                throw new Exception(String.Format(Locale.getTRFromKey("Settings::DirectoryFailed"), Locale.getTRFromKey("Directories::Players")));
            }
            if (!directoryAssert(worldsSavesLocation))
            {
                throw new Exception(String.Format(Locale.getTRFromKey("Settings::DirectoryFailed"), Locale.getTRFromKey("Directories::Worlds")));
            }
        }
        private static bool directoryAssert(string directory)
        {
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

    }
}
