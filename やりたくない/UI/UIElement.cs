using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.UI
{
    /// <summary>
    /// represents a general UI element
    /// </summary>
    internal abstract class UIElement
    {
        public UIElement()
        {
            onHover += delegate { hovered = true; };
            onUnHover += delegate { hovered = false; };
        }

        public Rectangle lastRenderedSize;

        /// <summary>
        /// handles click event, should be fired on all clicks on the valid window
        /// </summary>
        /// <param name="e"></param>
        public void click(EventArgs e)
        {
            UIElementClickEventArgs args = e as UIElementClickEventArgs;
            if (lastRenderedSize.Intersects(args.mouseLocation))
                onClick(e);
            else
                onUnclick(e);
        }

        protected event yaritakunaiEventHandlerE onClick = delegate { };
        protected event yaritakunaiEventHandlerE onUnclick = delegate { };

        public void hover() => onHover();
        public void unHover() => onUnHover();

        protected event yaritakunaiEventHandler onHover = delegate { };
        protected event yaritakunaiEventHandler onUnHover = delegate { };

        public bool hovered = false;

        public virtual void update()
        {
            bool hoveredThisFrame = (Input.mouseTip.Intersects(lastRenderedSize) || Input.pseudoMouseTip.Intersects(lastRenderedSize));
            if (!hovered && hoveredThisFrame)
            {
                onHover();
            }
            else if (hovered && !hoveredThisFrame)
            {
                onUnHover();
            }
        }

        public bool interactable = false;
        public virtual void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {
            if (context.isHorizontal)
            {
                //Draw line for error.
                spriteBatch.Draw(Main.notverymagicpixel, context.nextControlRenderingLocation, null, UIStyleUnification.errorLineColor, 0f, Vector2.Zero, new Vector2(1, UIStyleUnification.errorLineHeight), SpriteEffects.None, layer);
                //set the last render location as the line
                lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, 1, UIStyleUnification.errorLineHeight);
            }
            else
            {
                int errorLineWidth = context.width;
                spriteBatch.Draw(Main.notverymagicpixel, context.nextControlRenderingLocation, null, UIStyleUnification.errorLineColor, 0f, Vector2.Zero, new Vector2(errorLineWidth, 1), SpriteEffects.None, layer);
                //set the element's render location as the line
                lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, errorLineWidth, 1);

            }
        }

        /// <summary>
        /// draws border and background box in either the default background colors or in provided colors
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="context"></param>
        /// <param name="specialBorderColor"></param>
        protected void drawBorderAndBackgroundBox(SpriteBatch spriteBatch, UIRenderingContext context, float layer, Color? borderColor = null, Color? backgroundColor = null)
        {
            borderColor = borderColor ?? UIStyleUnification.borderColor;
            spriteBatch.Draw(Main.notverymagicpixel, lastRenderedSize, null, borderColor.Value, 0f, Vector2.Zero, SpriteEffects.None, layer + .0002f);
            backgroundColor = backgroundColor ?? UIStyleUnification.elementBackground;
            spriteBatch.Draw(Main.notverymagicpixel, new Rectangle((int)context.nextControlRenderingLocation.X + UIStyleUnification.border, (int)context.nextControlRenderingLocation.Y + UIStyleUnification.border, lastRenderedSize.Width - 2 * UIStyleUnification.border, lastRenderedSize.Height - 2 * UIStyleUnification.border), null, backgroundColor.Value, 0, Vector2.Zero, SpriteEffects.None, layer + .0001f);
        }
    }
}
