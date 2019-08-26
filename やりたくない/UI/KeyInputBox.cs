using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.UI
{
    class KeyInputBox : UIElement
    {
        public KeyInputBox(Keys initialValue, yaritakunaiEventHandlerO OnKeyChange = null)
        {
            keyName = initialValue.ToString();
            onClick += _click;
            onUnclick += _unclick;
            onKeyChange += OnKeyChange;
            onKeyChange += delegate (object key) { keyName = key.ToString(); };
        }

        string keyName;
        event yaritakunaiEventHandlerO onKeyChange = delegate { };
        bool waitingForInput = false;

        public void _click(EventArgs e)
        {
            //pull stuff out of the eventargs
            UIElementClickEventArgs args = e as UIElementClickEventArgs;
            bool isControllerClick = args.isControllerClick;
            //controller cannot map with onTextInput so don't bother with activating the box if it's controller-clicked
            if (isControllerClick)
                return;
            Main.refmain.Window.TextInput += getSingleKeypress;
            waitingForInput = true;
        }
        public void _unclick(EventArgs e)
        {
            waitingForInput = false;
            Main.refmain.Window.TextInput -= getSingleKeypress;
        }



        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {
            Color colour = waitingForInput ? UIStyleUnification.elementSelect : UIStyleUnification.defaultText;
            Vector2 size = Main.UIFont.MeasureString(keyName);
            lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, (int)size.X + UIStyleUnification.borderSize * 3, (int)size.Y + UIStyleUnification.borderSize * 2);
            drawBorderAndBackgroundBox(spriteBatch, context, layer);
            spriteBatch.DrawString(Main.UIFont, keyName, context.nextControlRenderingLocation + new Vector2(UIStyleUnification.borderSize), colour, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }


        void getSingleKeypress(object sender, TextInputEventArgs e)
        {
            onKeyChange(e.Key);
            waitingForInput = false;
            Main.refmain.Window.TextInput -= getSingleKeypress;
        }
    }
}
