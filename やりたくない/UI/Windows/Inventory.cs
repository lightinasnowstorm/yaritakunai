using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI.Windows
{
    internal static class Inventory
    {
        public static UIWindow 作る(Vector2 場所, bool tickedUp = false, bool visible = true)
        {
            UIWindow window = new UIWindow(場所, "inventory", 5, 8, (uint)windowTopButtons.titlebar | (uint)windowTopButtons.tickDown);
            const int inventoryRows = 5;
            const int slotsInRow = Player.inventorySlots / inventoryRows;
            for (int i = 0; i < inventoryRows; i++)
            {
                HorizontalGroupBox row = new HorizontalGroupBox(8, 0);
                for (int j = 0; j < slotsInRow; j++)
                {
                    Itembox box = new Itembox((Item)null, true);
                    int loc = slotsInRow * i + j;
                    Main.refmain.localPlayerLoaded += delegate { box.itemHolder = Main.currentPlayer.inventory[loc]; };
                    row.Add(box);

                }
                window.Add(row);
            }

            window.Visible = visible;
            window.tickedUp = tickedUp;
            return window;
        }

    }
}
