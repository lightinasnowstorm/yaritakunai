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
    /// Is a box holding an item. (Mainly for inventory?)
    /// </summary>
    class Itembox : UIElement
    {
        public Itembox(Holder<Item> holder, bool Grabbable = true)
        {
            itemHolder = holder;
            grabbable = Grabbable;
            onClick += _click;
        }
        public Itembox(Item Inside, bool Grabbable = false)
        {
            itemHolder.held = Inside;
            grabbable = Grabbable;
            onClick += _click;
        }
        public Holder<Item> itemHolder = new Holder<Item>(null);
        bool grabbable;

        private void _click(EventArgs e)
        {

            //pull stuff out of the eventargs
            UIElementClickEventArgs args = e as UIElementClickEventArgs;
            Rectangle loc = args.mouseLocation;
            bool isControllerClick = args.isControllerClick;
            //swap items from the mouse to the box if it's allowed
            if (grabbable)
                if (isControllerClick)
                {
                    Item tempVal = Main.refmain.pseudoCursorItem;
                    Main.refmain.pseudoCursorItem = itemHolder.held;
                    itemHolder.held = tempVal;
                }
                else
                {
                    Item tempVal = Main.refmain.cursorItem;
                    Main.refmain.cursorItem = itemHolder.held;
                    itemHolder.held = tempVal;
                }
        }

        const int boxSide = 32;
        const int totalBoxSide = boxSide + UIStyleUnification.borderSize * 2;

        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext context, float layer)
        {

            lastRenderedSize = new Rectangle((int)context.nextControlRenderingLocation.X, (int)context.nextControlRenderingLocation.Y, totalBoxSide, totalBoxSide);
            if (grabbable)
                drawBorderAndBackgroundBox(spriteBatch, context, layer, UIStyleUnification.selectOutline);
            else
                drawBorderAndBackgroundBox(spriteBatch, context, layer);

            if (itemHolder.held != null)
                spriteBatch.Draw(Item.itemTextures[itemHolder.held.id], new Rectangle((int)context.nextControlRenderingLocation.X + UIStyleUnification.borderSize, (int)context.nextControlRenderingLocation.Y + UIStyleUnification.borderSize, boxSide, boxSide), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layer + .00001f);

        }
    }
}
