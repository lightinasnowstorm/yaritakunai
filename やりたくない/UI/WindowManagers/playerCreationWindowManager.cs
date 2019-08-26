using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace やりたくない.UI.WindowManagers
{
    internal static class playerCreationWindowManager
    {
        public static WindowManager 作る()
        {
            WindowManager manager = new WindowManager(WindowManager.windowManagers.playerCreation);
            manager.addWindow(Windows.playerCreationWindow.作る(new Microsoft.Xna.Framework.Vector2(20, 20)));
            return manager;
        }
    }
}
