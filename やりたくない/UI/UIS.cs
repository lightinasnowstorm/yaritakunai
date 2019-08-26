//preprocessor variables
#define HIDE_UIS
//#define HIDE_TEST_UIS
#define HIDE_CHEAT_UIS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace やりたくない.UI
{
    /// <summary>
    /// Contains the UIs for the game.
    /// Mostly menus.
    /// </summary>
    static class UIS
    {
        public static bool anyClicked;
        /// <summary>
        /// Initializes all menus.
        /// </summary>
        public static void init()
        {
            Locale.onLocaleLoadOrChange += reset;

            resetLanguageSelect();
            reset();

            Main.characterSelectSwitchHandler += setCharacterSelect;
        }
        /// <summary>
        /// Resets all menus that might ever need resetting.
        /// </summary>
        public static void reset()
        {
            resetMainMenu();
            resetPauseMenu();
        }
        //All menus are contained in a logical order.

        #region language select
        public static void resetLanguageSelect()
        {
            langSelect.clearOptions(0);
            //Add language select menu in the else, as we only need it if we don't already have settings set.
            //Use the enum value names.
            string[] languageNames = Enum.GetNames(typeof(Languages));
            foreach (string language in languageNames)
            {
                langSelect.appendOption(0, language.Replace('_', ' '), handleLanguageClick);
            }
        }
        private static yaritakunaiEventHandlerOE handleLanguageClick = delegate (object sender, EventArgs e)
          {
              int i = (e as SelectionMenuClickEventArgs).i;
              int j = (e as SelectionMenuClickEventArgs).j;
              //switch the locale to the language given by the clicked language.
              Locale.switchLocaleTo((Languages)Enum.Parse(typeof(Languages), langSelect.options[i][j].text.Replace(' ', '_')));
              Transitions.SetupAndBegin((sender as Main).GraphicsDevice, (sender as Main), Transitions.Fade, screens.langSelect, screens.title);
          };
        public static selectionmenu langSelect = new selectionmenu(1, 40, 50);
        #endregion

        #region main menu
        static void resetMainMenu()
        {
            mainMenu.title = Main.toCapitalCase(Locale.getTRFromKey("Game::Name"));
            mainMenu.clearOptions(0);
            mainMenu.appendOption(0, Locale.getTRFromKey("MainMenu::Start"),
                delegate (object sender, EventArgs e)
                {
                    Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.title, screens.characterSelect);
                });
            mainMenu.appendOption(0, Locale.getTRFromKey("MainMenu::Settings"),
                delegate (object sender, EventArgs e)
                {
                    //does nothing yet.
                });
            mainMenu.appendOption(0, Locale.getTRFromKey("MainMenu::MultiplayerOption"),
                delegate (object sender, EventArgs e)
                {
                    //also does nothing, it's only singleplayer (thus far)
                });
            mainMenu.appendOption(0, Locale.getTRFromKey("Game::Exit"),
                delegate (object sender, EventArgs e)
                {
                    (sender as Main).Exit();
                });
        }
        public static selectionmenu mainMenu = new selectionmenu(1, 27, 0, Main.toCapitalCase(Locale.getTRFromKey("Game::Name")), new Vector2(0, 50))
        {
            center = new Vector2(0, 120)
        };
        #endregion

        static void setCharacterSelect()
        {
            characterSelect.clearOptions(0);
            DirectoryInfo info = new DirectoryInfo(Settings.playerSavesLocation);
            var files = info.EnumerateFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".ghb")
                {
                    string name = Player.playerNameFromFile(file.FullName);
                    if (name == null)
                        continue;
                    characterSelect.appendOption(0, name,
                     delegate (object sender, EventArgs e)
                     {
                         Player loadedPlayer = Player.load(file.FullName);
                         if (loadedPlayer == null)//have some sort of notification to tell the user that the load failed
                             return;
                         (sender as Main).setPlayer(loadedPlayer);
                         Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.characterSelect, screens.mainGame);
                     });
                }
            }
            //sub menu
            cssubmenu.appendOption(0, Locale.getTRFromKey("Game::Back"),
                delegate (object sender, EventArgs e)
                {
                    Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.characterSelect, screens.title);
                });
            cssubmenu.appendOption(1, Locale.getTRFromKey("Game::Create"),
                delegate (object sender, EventArgs e)
                {
                    Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.characterSelect, screens.characterCreate);
                });

        }
        public static selectionmenu cssubmenu = new selectionmenu(2, 0, 100)
        {
            center = new Vector2(0, 200)
        };
        public static selectionmenu characterSelect = new selectionmenu(1, 30, 0)
        {
            center = new Vector2(0, -200)
        };

        #region pause menu
        static void resetPauseMenu()
        {
            pauseMenu.clearOptions(0);
            pauseMenu.clearOptions(1);
            pauseMenu.appendOption(0, Locale.getTRFromKey("Game::Resume"),
                delegate (object sender, EventArgs e)
                {
                    Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.gamePaused, screens.mainGame);
                });
            pauseMenu.appendOption(0, Locale.getTRFromKey("MainMenu::Settings"),
                delegate (object sender, EventArgs e)
                {
                    //not implemented yet.
                });
            pauseMenu.appendOption(1, Locale.getTRFromKey("Pause::QuitMainMenu"),
                delegate (object sender, EventArgs e)
                {
                    Main.currentPlayer.save();
                    Transitions.SetupAndBegin((sender as Main).GraphicsDevice, sender as Main, Transitions.Fade, screens.gamePaused, screens.title);
                });
        }
        public static selectionmenu pauseMenu = new selectionmenu(2, 30, 200);
        #endregion
    }
}
