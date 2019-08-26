//#define FORCE_RESET_UIS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Newtonsoft.Json;

namespace やりたくない.UI
{
    internal class WindowManager
    {
        windowManagers type;
        /// <summary>
        /// the manager window manages the other windows: null if the other windows are unmanaged
        /// </summary>
        public UIWindow managerWindow = null;
        /// <summary>
        /// Supports theoretically infinite windows, the limit is 99 because of rendering concerns
        /// </summary>
        List<UIWindow> windows = new List<UIWindow>();

        public WindowManager(windowManagers type, UIWindow managerWindow = null)
        {
            this.managerWindow = managerWindow;
            this.type = type;
        }

        public void addWindow(UIWindow window)
        {
            if (windows.Count < 99)
            {
                windows.Add(window);
            }
        }

        public static void init()
        {
            //create all necessary window managers
            Main.refmain.gameWM = WindowManagers.gameWindowManager.作る();
            Locale.onLocaleLoadOrChange += Main.refmain.gameWM.forceResetWindows;
            Main.refmain.settingsWM = new WindowManager(windowManagers.settings);
            Main.refmain.playerCreationWM = WindowManagers.playerCreationWindowManager.作る();
            Locale.onLocaleLoadOrChange += Main.refmain.playerCreationWM.forceResetWindows;
        }


        public void update(GameTime gameTime)
        {
            foreach (UIWindow window in windows)
            {
                window.update(gameTime);
            }
            if (Input.mouseMainButton)
            {
                click(Input.mouseTip);
            }
            if (Input.pseudoMouseClicked)
            {
                click(Input.pseudoMouseTip, true);
            }
        }

        private void forceResetWindows()
        {
            switch (type)
            {
                case windowManagers.game:
                    windows = WindowManagers.gameWindowManager.作る().windows;
                    break;
                case windowManagers.playerCreation:
                    windows = WindowManagers.playerCreationWindowManager.作る().windows;
                    break;
            }
        }

        private void click(Rectangle mouseTip, bool controllerClick = false)
        {
            //check windows in order of overlap: first to last
            foreach (UIWindow win in windows)
            {
                if ((win.lastRenderedLocation.Intersects(mouseTip) && !win.tickedUp) || win.titleBarRectangle.Intersects(mouseTip))
                {
                    focusWindow(win);
                    win.click(mouseTip, controllerClick);
                    UIS.anyClicked = true;
                    return;
                }
            }
            windows[0].isFocused = false;
        }

        private void focusWindow(UIWindow window)
        {
            focusWindow(windows.IndexOf(window));
        }
        private void focusWindow(int numInList)
        {
            windows[0].isFocused = false;
            UIWindow win = windows[numInList];
            windows.RemoveAt(numInList);
            windows.Insert(0, win);
            windows[0].isFocused = true;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            float layer = 0;
            foreach (UIWindow window in windows)
            {
                window.draw(spriteBatch, layer);
                layer += .01f;
            }
            spriteBatch.End();
        }




        //file saving/loading
        public bool tryReloadWindowManager()
        {
#if FORCE_RESET_UIS
            return false;
#endif
            try
            {
                dynamic loadedWindows = JsonConvert.DeserializeObject(File.ReadAllText(Settings.savesLocation + type.ToString() + "WM.json"));
                foreach (var windowData in loadedWindows)
                {
                    Vector2 location = windowData.location;
                    bool visible = windowData.visible;
                    bool tickedUp = windowData.tickedUp;
                    UIWindow window = null;
                    switch (windowData.name.Value)
                    {
                        case "test":
                            window = Windows.testUI.作る(location, tickedUp, visible);
                            break;
                        case "cheats":
                            window = Windows.CheatUI.作る(location, tickedUp, visible);
                            break;
                        case "inventory":
                            window = Windows.Inventory.作る(location, tickedUp, visible);
                            break;
                        case "keybinds":
                            window = Windows.Keybinds.作る(location, tickedUp, visible);
                            break;
                        case "player creation":
                            window = Windows.playerCreationWindow.作る(location, tickedUp, visible);
                            break;
                        default:
                            //silent failure
                            break;
                    }
                    if (window == null)
                        continue;
                    window.lastWidth = windowData.lastWidth;
                    windows.Add(window);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void saveWindowManager()
        {
            var windowLocations = (from window in windows select new { name = window.Name, window.location, visible = window.Visible, window.tickedUp, window.lastWidth }).ToList();
            string allJson = JsonConvert.SerializeObject(windowLocations, Settings.formatting);
            File.WriteAllText(Settings.savesLocation + type.ToString() + "WM.json", allJson);
        }

        public enum windowManagers
        {
            game,
            settings,
            playerCreation
        }
    }
}
