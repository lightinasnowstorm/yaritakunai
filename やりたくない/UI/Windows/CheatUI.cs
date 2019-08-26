using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI.Windows
{
    internal static class CheatUI
    {
        public static UIWindow 作る(Vector2 場所, bool tickedUp = false, bool visible = true)
        {
            Textbox xBox = new Textbox(30, "0");
            Textbox yBox = new Textbox(30, "0");
            HorizontalGroupBox locBox = new HorizontalGroupBox(0, 0);
            locBox.Add(new UIText("X:"));
            locBox.Add(xBox);
            locBox.Add(new UIText("Y:"));
            locBox.Add(yBox);
            Textbox itemIDBox = new Textbox(30, "0");
            Button spawnNewItem = new Button("Spawn an item", delegate (EventArgs e)
            {
                tryToSpawnItem(itemIDBox.text, xBox.text, yBox.text);
            });
            Itembox spawnedItemBox = new Itembox((Item)null, true);
            Button boxItemSpawner = new Button("Box item", delegate (EventArgs e)
            {
                spawnedItemBox.itemHolder.held = tryToParseItem(itemIDBox.text);
            });
            UIWindow window = new UIWindow(場所, "cheats", 5, 8, (uint)windowTopButtons.titlebar | (uint)windowTopButtons.tickDown | (uint)windowTopButtons.close)
            {
                new UIText("Coordinates:"),
                locBox,
                new UIText("Item ID:"),
                itemIDBox,
                spawnNewItem,
                spawnedItemBox,
                boxItemSpawner
            };
            window.Visible = visible;
            window.tickedUp = tickedUp;
            return window;

        }
        private static Item tryToParseItem(string itemID)
        {
            if (UInt16.TryParse(itemID, out ushort parseditemID))
            {
                if (parseditemID < Item.numItems)
                    return new Item(parseditemID);
            }
            return null;
        }
        private static void tryToSpawnItem(string itemID, string x, string y)
        {
            Item item = tryToParseItem(itemID);
            if (item != null && Single.TryParse(x, out float parsedx) && Single.TryParse(y, out float parsedy))
            {
                new ItemInWorld(item, new Vector2(parsedx, parsedy));
            }
        }
    }
}
