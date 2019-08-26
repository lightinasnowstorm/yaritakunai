using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.UI
{
    class Button : UIElement
    {
        public Button(string Text, yaritakunaiEventHandlerE handler = null)
        {
            text = Text;
            onClick += handler;
            interactable = true;
        }

        string text;


        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {
            Vector2 size = Main.UIFont.MeasureString(text);
            lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, (int)size.X + UIStyleUnification.borderSize * 3, (int)size.Y + UIStyleUnification.borderSize * 2);
            Color backgroundColor = hovered ? UIStyleUnification.interactHoverBackground : UIStyleUnification.interactBackground;
            drawBorderAndBackgroundBox(spriteBatch, context, layer, UIStyleUnification.interactBorder, backgroundColor);
            spriteBatch.DrawString(Main.UIFont, text, context.nextControlRenderingLocation + new Vector2(UIStyleUnification.borderSize), UIStyleUnification.defaultText, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }

    }
}
