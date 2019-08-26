using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace やりたくない.UI.Windows
{
    internal static class Keybinds
    {
        public static UIWindow 作る(Vector2 場所, bool tickedUp = false, bool visible = true)
        {
            var window = new UIWindow(場所, "keybinds", 5, 8, (uint)windowTopButtons.titlebar | (uint)windowTopButtons.tickDown | (uint)windowTopButtons.removeToParent | (uint)windowTopButtons.close, 100)
            {
                new UIText(Locale.getTRFromKey("Keybind::Jump")),
                new KeyInputBox(Input.arrange.jump, delegate (object key) { Input.arrange.jump = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Boost")),
                new KeyInputBox(Input.arrange.boost, delegate (object key) { Input.arrange.boost = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Pause")),
                new KeyInputBox(Input.arrange.pause, delegate (object key) { Input.arrange.pause = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Left")),
                new KeyInputBox(Input.arrange.left, delegate (object key) { Input.arrange.left = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Right")),
                new KeyInputBox(Input.arrange.right, delegate (object key) { Input.arrange.right = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Up")),
                new KeyInputBox(Input.arrange.up, delegate (object key) { Input.arrange.up = (Keys)key; }),
                new UIText(Locale.getTRFromKey("Keybind::Down")),
                new KeyInputBox(Input.arrange.down, delegate (object key) { Input.arrange.down = (Keys)key; })
            };
            window.Visible = visible;
            window.tickedUp = tickedUp;
            return window;
        }
    }
}
