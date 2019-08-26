using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI.WindowManagers
{
    internal static class gameWindowManager
    {
        public static WindowManager 作る()
        {
            WindowManager manager = new WindowManager(WindowManager.windowManagers.game);
            bool isReloaded = manager.tryReloadWindowManager();
            if (!isReloaded)
            {
                //failed to reload it, therefore need to make it.

                //テストのWindowを作る
                manager.addWindow(Windows.testUI.作る(new Vector2(50, 250)));
                manager.addWindow(Windows.Inventory.作る(new Vector2(150, 50)));
                manager.addWindow(Windows.CheatUI.作る(new Vector2(750, 50)));
            }
            return manager;
        }
    }
}
