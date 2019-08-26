using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.UI
{
    class UIWindow : List<UIElement>
    {
        string name;
        public string Name => name;

        public bool isFocused;
        public Rectangle titleBarRectangle = Rectangle.Empty;
        public Rectangle lastRenderedLocation;
        /// <summary>
        /// Controls the visibility and interactibility of a window.
        /// </summary>
        public bool Visible = true;


        public static Texture2D tickDownBack, tickDownSymbol, removeToParent, close;
        public static Texture2D tickDownBorder, removeToParentBorder, closeBorder;
        /// <summary>
        /// height of titlebar, in pixels
        /// </summary>
        const int titleBarHeight = 20;
        /// <summary>
        /// size of buttons, in pixels
        /// </summary>
        const int buttonSize = 16;
        /// <summary>
        /// Amount of pixel space a button takes up
        /// </summary>
        const int buttonLocationSize = 2 * buttonPad + buttonSize;
        /// <summary>
        /// padding on buttons, in pixels
        /// </summary>
        const int buttonPad = (titleBarHeight - buttonSize) / 2;
        /// <summary>
        /// amount rounded on a "up" window, in pixels.
        /// </summary>
        const int endRounding = 10;
        /// <summary>
        /// the buttons on the window, is all the ones from the enum
        /// </summary>
        public uint windowButtons;
        public bool tickedUp = false;

        int xPadding, yPadding;

        public Vector2 location;

        int minimumWidth;

        public uint lastWidth;
        uint lastPaddedWidth => lastWidth + (uint)xPadding * 2;

        /// <summary>
        /// initializes a UI window.
        /// </summary>
        /// <param name="WindowButtons">or'd values of the buttons on the top.</param>
        public UIWindow(Vector2 Location, string Name, uint YPadding, uint XPadding, uint WindowButtons = 0, int MinimumWidth = -1, uint DefaultWidth = 200)
        {
            location = Location;
            windowButtons = WindowButtons;
            yPadding = (int)YPadding;
            xPadding = (int)XPadding;
            lastWidth = DefaultWidth;
            minimumWidth = MinimumWidth;

            name = Name;
        }

        public void Add(params UIElement[] elements)
        {
            foreach (UIElement element in elements)
                base.Add(element);
        }

        bool mouseHeld;
        bool controllerHeld;
        int titlebarY => (int)location.Y - titleBarHeight;
        bool tickDownHovered, removeToParentHovered, closeHovered;
        int buttonYLocation => titlebarY + buttonPad;
        Vector2 tickDownLocation => new Vector2((int)location.X + endRounding, buttonYLocation);
        Rectangle tickDownRectangle => new Rectangle((int)location.X + endRounding, buttonYLocation, buttonSize, buttonSize);
        int rightEdgeLocation => (int)(location.X + lastPaddedWidth - endRounding);
        Vector2 removeToParentLocation => new Vector2(rightEdgeLocation - 2 * buttonLocationSize, buttonYLocation);
        Rectangle removeToParentRectangle => new Rectangle(rightEdgeLocation - 2 * buttonLocationSize, buttonYLocation, buttonSize, buttonSize);
        Vector2 closeLocation => new Vector2(rightEdgeLocation - buttonLocationSize, buttonYLocation);
        Rectangle closeRectangle => new Rectangle(rightEdgeLocation - buttonLocationSize, buttonYLocation, buttonSize, buttonSize);

        public void update(GameTime gameTime)
        {
            if (!Visible)
                return;

            foreach (UIElement element in this)
            {
                element.update();
            }


            //hovers, we can only have them if titlebar
            if ((windowButtons & (uint)windowTopButtons.titlebar) == (uint)windowTopButtons.titlebar)
            {
                if ((windowButtons & (uint)windowTopButtons.tickDown) == (uint)windowTopButtons.tickDown)
                {
                    tickDownHovered = Input.mouseTip.Intersects(tickDownRectangle)
                        || Input.pseudoMouseTip.Intersects(tickDownRectangle);
                }

                if ((windowButtons & (uint)windowTopButtons.removeToParent) == (uint)windowTopButtons.removeToParent)
                {
                    removeToParentHovered = Input.mouseTip.Intersects(removeToParentRectangle)
                        || Input.pseudoMouseTip.Intersects(removeToParentRectangle);
                }

                if ((windowButtons & (uint)windowTopButtons.close) == (uint)windowTopButtons.close)
                {
                    closeHovered = Input.mouseTip.Intersects(closeRectangle)
                        || Input.pseudoMouseTip.Intersects(closeRectangle);
                }
            }
            if (mouseHeld)
            {
                //move the window the same amount as the mouse.
                location += Input.mouseDelta;
                if (Input.mouseMainButtonReleased)
                    mouseHeld = false;
            }
            if (controllerHeld)
            {
                //move the window the same amount as the cursor
                location += Input.pseudoMouseDelta;
                if (Input.pseudoMouseReleased)
                    controllerHeld = false;
            }
        }

        public void click(Rectangle mouseTip, bool isControllerClick)
        {
            //if this window is clicked...

            //check each element to see
            if (!tickedUp)
            {
                PlayerControllers.PlayerInputController.inputBlockFromTextbox = false;
                foreach (UIElement element in this)
                {
                    element.click(new UIElementClickEventArgs(mouseTip, isControllerClick));
                }
            }

            //if there is a titlebar..
            if ((windowButtons & (uint)windowTopButtons.titlebar) == (uint)windowTopButtons.titlebar)
            {
                //if there is tickdown button check if it's clicked
                //tickdown takes precedence...
                if ((windowButtons & (uint)windowTopButtons.tickDown) == (uint)windowTopButtons.tickDown &&
                    mouseTip.Intersects(tickDownRectangle))
                {
                    //swap the value of tickedUp.
                    tickedUp = !tickedUp;
                }
                else if ((windowButtons & (uint)windowTopButtons.close) == (uint)windowTopButtons.close &&
                    mouseTip.Intersects(closeRectangle))
                {
                    //close the UI window.
                    //iunno how, tho.
                    //we have a way: the Visible parameter.
                    Visible = false;
                }
                else if ((windowButtons & (uint)windowTopButtons.removeToParent) == (uint)windowTopButtons.removeToParent &&
                    mouseTip.Intersects(removeToParentRectangle))
                {
                    //remove to parent, whatever that means.
                }
                else if (mouseTip.Intersects(titleBarRectangle))
                {
                    //grab titlebar to move
                    if (Input.mouseMainButton && !controllerHeld)
                    {
                        //Grab with mouse.
                        mouseHeld = true;
                    }
                    else if (Input.pseudoMouseClicked && !mouseHeld)
                    {
                        //Grab with controller.
                        controllerHeld = true;
                    }
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, float layer)
        {
            if (!Visible)
                return;
            Vector2 renderloc = location;
            //don't move down with titlebar, also draw a border
            //if there is a titlebar, move down where the main window is drawn.
            if ((windowButtons & (uint)windowTopButtons.titlebar) == (uint)windowTopButtons.titlebar)
            {
                //renderloc.Y += titleBarHeight;
                //Also draw titlebar.
            }
            Color borderColor = isFocused ? UIStyleUnification.windowBorderColorFocused : UIStyleUnification.windowBorderColor;
            if (!tickedUp)
            {
                UIRenderingContext context = new UIRenderingContext(new Vector2(renderloc.X + xPadding, renderloc.Y + yPadding), true, minimumWidth);
                foreach (UIElement element in this)
                {
                    //draw each element
                    element.draw(spriteBatch, ref context, layer);

                    //Do this regardless of element type.
                    context.nextControlRenderingLocation.Y += element.lastRenderedSize.Height;
                    context.nextControlRenderingLocation.Y += yPadding;
                    context.speculateWidth(element.lastRenderedSize.Width);
                }
                //draw the background of the window based on total elements drawn.
                //which is in the UIRenderingContext
                int width = context.width;
                //save this width for later
                lastWidth = (uint)width;
                int paddedWidth = width + 2 * xPadding;
                //Only need to add one Y padding due to it being added at the last element.
                int height = context.height + yPadding;

                lastRenderedLocation = new Rectangle((int)renderloc.X, (int)renderloc.Y, paddedWidth, height);
                //draw the background of the window
                spriteBatch.Draw(Main.notverymagicpixel, lastRenderedLocation, null, UIStyleUnification.windowBackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, layer + .001f);
                //draw border
                spriteBatch.Draw(Main.notverymagicpixel, new Rectangle(lastRenderedLocation.X - UIStyleUnification.windowBorder, lastRenderedLocation.Y - UIStyleUnification.windowBorder, lastRenderedLocation.Width + 2 * UIStyleUnification.windowBorder, lastRenderedLocation.Height + 2 * UIStyleUnification.windowBorder), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, layer + 0.002f);

            }

            //Draw the titlebar.
            if ((windowButtons & (uint)windowTopButtons.titlebar) == (uint)windowTopButtons.titlebar)
            {
                //magic pixel is temporary, want to have a textured, 3d-looking, including on the ends, so it can just be drawn to the rectangle without any worries.
                int titleBarY = (int)location.Y - titleBarHeight;
                titleBarRectangle = new Rectangle((int)location.X, titleBarY, (int)lastPaddedWidth, titleBarHeight);
                spriteBatch.Draw(Main.notverymagicpixel, titleBarRectangle, null, UIStyleUnification.titlebarColor, 0f, Vector2.Zero, SpriteEffects.None, layer + 0.001f);
                //draw an outline for it
                spriteBatch.Draw(Main.notverymagicpixel, new Rectangle(titleBarRectangle.X - UIStyleUnification.windowBorder, titleBarRectangle.Y - UIStyleUnification.windowBorder, titleBarRectangle.Width + 2 * UIStyleUnification.windowBorder, titleBarRectangle.Height + 2 * UIStyleUnification.windowBorder), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, layer + 0.002f);

                //titlebar is required to have buttons.

                //down tick
                if ((windowButtons & (uint)windowTopButtons.tickDown) == (uint)windowTopButtons.tickDown)
                {
                    spriteBatch.Draw(tickDownBack, tickDownLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer + .0003f);
                    spriteBatch.Draw(tickDownSymbol, tickDownLocation, null, Color.White, 0f, Vector2.Zero, 1f, tickedUp ? SpriteEffects.None : SpriteEffects.FlipVertically, layer + .0002f);
                    if (tickDownHovered)
                        spriteBatch.Draw(tickDownBorder, tickDownLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer + .0001f);
                }

                //close to parent (minimise?)
                if ((windowButtons & (uint)windowTopButtons.removeToParent) == (uint)windowTopButtons.removeToParent)
                {
                    spriteBatch.Draw(removeToParent, removeToParentLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer + 0.0001f);
                    if (removeToParentHovered)
                        spriteBatch.Draw(removeToParentBorder, removeToParentLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);

                }

                //close
                if ((windowButtons & (uint)windowTopButtons.close) == (uint)windowTopButtons.close)
                {
                    spriteBatch.Draw(close, closeLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer + 0.0001f);
                    if (closeHovered)
                        spriteBatch.Draw(closeBorder, closeLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);

                }


            }


        }
    }
    /// <summary>
    /// window buttons
    /// </summary>
    enum windowTopButtons : uint
    {
        nothing = 0b0,
        titlebar = 0b1,
        tickDown = 0b10,
        close = 0b100,
        removeToParent = 0b1000
    }
}
