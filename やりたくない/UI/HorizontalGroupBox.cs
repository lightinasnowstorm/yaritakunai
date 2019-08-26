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
    /// A horizontal group box groups items inside of it horizontally, instead of vertically like a window's default
    /// </summary>
    class HorizontalGroupBox : UIElement
    {
        public void Add(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                this.elements.Add(element);
        }
        public void Clear()
        {
            elements.Clear();
        }

        List<UIElement> elements = new List<UIElement>();
        public HorizontalGroupBox(int XPadding, int YPadding)
        {
            xPadding = XPadding;
            yPadding = YPadding;
            //click on any click
            onClick += _click;
            onUnclick += _click;
        }

        public void _click(EventArgs e)
        {
            //try to click all contained elements
            foreach (UIElement element in elements)
            {
                element.click(e);
            }
        }

        public override void update()
        {
            foreach (UIElement element in elements)
            {
                element.update();
            }
            base.update();
        }

        int xPadding, yPadding;

        public override void draw(SpriteBatch spriteBatch, ref UIRenderingContext outerContext, float layer)
        {
            UIRenderingContext context = new UIRenderingContext(outerContext.nextControlRenderingLocation + new Vector2(xPadding, yPadding), false);
            foreach (UIElement element in elements)
            {
                //draw each element

                element.draw(spriteBatch, ref context, layer);
                //padding
                context.nextControlRenderingLocation.X += xPadding;
                context.nextControlRenderingLocation.X += element.lastRenderedSize.Width;
                context.speculateHeight(element.lastRenderedSize.Height);
            }
            //Only need to add one X padding due to it being added at the last element.
            int paddedWidth = context.width + xPadding;
            int height = context.height + 2 * yPadding;

            lastRenderedSize = new Rectangle((int)outerContext.nextControlRenderingLocation.X, (int)outerContext.nextControlRenderingLocation.Y, paddedWidth, height);

        }
    }
}
