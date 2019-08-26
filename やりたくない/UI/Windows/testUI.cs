using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace やりたくない.UI.Windows
{
    internal static class testUI
    {
        public static UIWindow 作る(Vector2 場所, bool tickedUp = false, bool visible = true)
        {
            HorizontalGroupBox gb = new HorizontalGroupBox(5, 8);
            gb.Add(new UIText("the fox says:"));
            gb.Add(new Textbox(50));
            gb.Add(new KeyInputBox(Keys.ImeNoConvert));
            gb.Add(new Textbox(50));
            gb.Add(new Button("button in horizontal box"));
            gb.Add(new Itembox(new Item(1), true));
            uint windowButtons = (uint)windowTopButtons.titlebar | (uint)windowTopButtons.tickDown | (uint)windowTopButtons.removeToParent | (uint)windowTopButtons.close;
            var window = new UIWindow(場所, "test", 5, 8, windowButtons)
            {
            new KeyInputBox(Input.arrange.jump, delegate (object key) { Input.arrange.jump = (Keys)key; }),
            new Textbox(150, "ナデシコ", "お名前は"),
            new Textbox(150, "ナデシコ", "お名前は"),
            new Button("Button", delegate
            {
                Console.WriteLine("button pressed");
            }),
            gb,
            new Itembox(new Item(0)),
            };
            window.Add(new Button("toggle window titlebar", delegate
                 {
                     window.windowButtons ^= windowButtons;
                 }));
            window.Visible = visible;
            window.tickedUp = tickedUp;
            return window;
        }
    }
}
