using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace やりたくない.UI
{
    class Textbox : UIElement
    {

        public Textbox(int Width, string initialValue = "", string PromptText = null)
        {
            onClick += _click;
            onUnclick += _unclick;
            width = Width;
            text = initialValue;
            promptText = PromptText;
            keytimer.Start();
        }

        int width;
        public string text;
        string promptText;

        bool selected = false;

        public void _click(EventArgs e)
        {
            Main.refmain.Window.TextInput += getKeyPress;
            selected = true;
            PlayerControllers.PlayerInputController.inputBlockFromTextbox = true;
        }
        public void _unclick(EventArgs e)
        {
            Main.refmain.Window.TextInput -= getKeyPress;
            selected = false;
            PlayerControllers.PlayerInputController.inputBlockFromTextbox |= false;
        }


        const int vertHeight = 12;


        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {
            Vector2 size = Main.UIFont.MeasureString(text);
            lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, width + UIStyleUnification.borderSize * 3, vertHeight + UIStyleUnification.borderSize * 2);
            if (selected)
                drawBorderAndBackgroundBox(spriteBatch, context, layer, UIStyleUnification.selectOutline);
            else
                drawBorderAndBackgroundBox(spriteBatch, context, layer);

            if (text != "")
                spriteBatch.DrawString(Main.UIFont, text, context.nextControlRenderingLocation + new Vector2(UIStyleUnification.borderSize), UIStyleUnification.defaultText, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
            else if (promptText != null)
                spriteBatch.DrawString(Main.SlantUIFont, promptText, context.nextControlRenderingLocation + new Vector2(UIStyleUnification.borderSize), UIStyleUnification.fadedText, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }


        //set this to 0 to enable IME through microsoft, but it lets key repeats go through
        const int keyRepeatTime = 15;//ms
        Stopwatch keytimer = new Stopwatch();

        private static HashSet<char> specialCharacters = new HashSet<char>() { '\n', '\t', '\b', '\r', '\u001b', '\u007f' };

        void getKeyPress(object sender, TextInputEventArgs e)
        {
            if (keytimer.ElapsedMilliseconds < keyRepeatTime)
                return;
            //0x8 is backspace, handle this correctly
            if (e.Character != 0x8 && e.Key != Settings.IMEKey && !specialCharacters.Contains(e.Character))
                text = IME.doIMEString(text, e.Character);
            else if (text != "" && e.Character == 0x8)
                text = text.Substring(0, text.Length - 1);
            if (text == "のりば" || text=="きょう" || text=="はれ")
            {
                text = IME.blah(text);
            }
            keytimer.Restart();
        }
    }
}
