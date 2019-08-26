using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace やりたくない.UI
{
    class UIText : UIElement
    {
        public UIText(string Text, Color? colour = null, SpriteFont Font = null)
        {
            text = Text;
            color = colour ?? UIStyleUnification.defaultText;
            font = Font ?? Main.UIFont;
        }
        string text;
        Color color;
        SpriteFont font;

        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {
            //Render the text in the location provided by the UIRenderingContext
            spriteBatch.DrawString(font, text, context.nextControlRenderingLocation, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
            Vector2 size = font.MeasureString(text);
            lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, (int)size.X, (int)size.Y);
        }


    }
}
