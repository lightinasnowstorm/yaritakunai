using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.UI
{
    internal class selectionmenu
    {
        internal class selectionMenuItem
        {
            public selectionMenuItem(string Text)
            {
                text = Text;
            }

            public string text;
            public yaritakunaiEventHandlerOE onClickHandler = delegate { };
        }
        bool hasTitle => title != null;
        public string title;
        /// <summary>
        /// position of title, relative to the top center of the screen.
        /// </summary>
        Vector2 titleLocation;
        /// <summary>
        /// Number of columns.
        /// </summary>
        int columns;
        public List<selectionMenuItem>[] options;
        public Vector2 center;
        int itemSeperation;
        int columnSeperation;
        Color unselectedColor = Color.White;
        //these colours should be based on the current cursor colours, not on the defaults.
        Color mouseSelectedColor = Color.Pink;
        Color controllerSelectedColor = Color.Aquamarine;
        public selectionmenu(int numColumns, int itemSep, int columnSep, string Title = null, Vector2? TitleLocation = null)
        {
            title = Title;
            titleLocation = TitleLocation ?? Vector2.Zero;
            center = new Vector2(0, 0);
            columns = numColumns;
            itemSeperation = itemSep;
            columnSeperation = columnSep;
            options = new List<selectionMenuItem>[numColumns];
            for (int i = 0; i < numColumns; i++)
            {
                options[i] = new List<selectionMenuItem>();
            }
        }
        public void update(Main main)
        {
            if (Input.mouseMainButton)
                mouseClick(main);
            if (Input.pseudoMouseClicked)
                controllerClick(main);
        }
        public void mouseClick(Main main)
        {
            clicky(main, isMouseSelected);
        }
        public void controllerClick(Main main)
        {
            clicky(main, isControllerSelected);
        }
        private void clicky(Main main, selectionthingy thingy)
        {
            //the menu needs to find what of it is clicked if anything of it is clicked at all
            //and call that handler.
            //traversing each list (and checking each element) is an inefficient way to do it but it's a way to do it.
            for (int i = 0; i < options.Length; i++)
            {
                for (int j = 0; j < options[i].Count; j++)
                {
                    if (thingy(i, j))
                    {
                        options[i][j].onClickHandler(main, new SelectionMenuClickEventArgs(i, j));
                    }
                }
            }
        }

        public void draw()
        {
            Main.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            if (hasTitle)
            {
                Main.spriteBatch.DrawString(Main.mainFont, title, new Vector2(titleLocation.X + Main.zeroedWindow.Width / 2 - Main.mainFont.MeasureString(title).X / 2, titleLocation.Y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < options[i].Count; j++)
                {
                    bool mSelected = isMouseSelected(i, j);
                    bool cSelected = isControllerSelected(i, j);
                    drawMenuItem(i, j, mSelected, cSelected);
                }
            }
            Main.spriteBatch.End();
        }
        private void drawMenuItem(int i, int j, bool mouseSelected, bool controllerSelected)
        {
            SpriteFont font = mouseSelected || controllerSelected ? Main.selectedFont : Main.mainFont;
            string usingText = mouseSelected || controllerSelected ? "[ " + options[i][j].text + " ]" : options[i][j].text;
            Vector2 location = new Vector2()
            {
                X = (i + 1 - columns) * columnSeperation + Main.center.X + center.X - ((font.MeasureString(usingText).X / 2)),
                Y = (j + 1 - options[i].Count) * itemSeperation + Main.center.Y + center.Y
            };
            //spaghettomatic
            //mouse color takes precidence
            Color color = mouseSelected || controllerSelected ? (mouseSelected ? mouseSelectedColor : controllerSelectedColor) : unselectedColor;
            Main.spriteBatch.DrawString(font, usingText, location, color);
        }
        public void appendOption(int column, string text, yaritakunaiEventHandlerOE handler = null)
        {
            options[column].Add(new selectionMenuItem(text));
            if (handler != null)
            {
                options[column].Last().onClickHandler += handler;
            }
        }
        public void clearOptions(int column)
        {
            options[column].Clear();
        }
        delegate bool selectionthingy(int i, int j);
        //this should be private as event handlers should be used over directly checking for a click and a spot.
        public bool isMouseSelected(int i, int j)
        {
            string usingText = options[i][j].text;
            //mouse figuring...
            Rectangle itemLocation = new Rectangle((int)((i + 1 - columns) * columnSeperation + Main.center.X + center.X - (Main.mainFont.MeasureString(usingText).X / 2)), (int)((j + 1 - options[i].Count) * itemSeperation + Main.center.Y + center.Y), (int)Main.mainFont.MeasureString(usingText).X, (int)Main.mainFont.MeasureString(usingText).Y);
            return itemLocation.Intersects(Input.mouseTip);
        }
        public bool isControllerSelected(int i, int j)
        {
            string usingText = options[i][j].text;
            //mouse figuring...
            Rectangle itemLocation = new Rectangle((int)((i + 1 - columns) * columnSeperation + Main.center.X + center.X - (Main.mainFont.MeasureString(usingText).X / 2)), (int)((j + 1 - options[i].Count) * itemSeperation + Main.center.Y + center.Y), (int)Main.mainFont.MeasureString(usingText).X, (int)Main.mainFont.MeasureString(usingText).Y);
            return itemLocation.Intersects(Input.pseudoMouseTip);
        }
    }
    internal class SelectionMenuClickEventArgs : EventArgs
    {
        public SelectionMenuClickEventArgs(int I, int J)
        {
            i = I;
            j = J;
        }
        public int i;
        public int j;
    }
}
