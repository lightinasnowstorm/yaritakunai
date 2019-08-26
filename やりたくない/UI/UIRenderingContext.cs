using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace やりたくない.UI
{
    /// <summary>
    /// Rendering context for UI.
    /// Struct as it *must* be assigned.
    /// However, all passes of theis type *must* be ref.
    /// </summary>
    struct UIRenderingContext
    {
        public UIRenderingContext(Vector2 startLocation, bool vertical = true, int MinimumWidth = -1, int MinimumHeight = -1)
        {
            initialLocation = startLocation;
            nextControlRenderingLocation = startLocation;
            minimumWidth = MinimumWidth;
            minimumHeight = MinimumHeight;
            spectulativeHeight = 0;
            spectulativeWidth = 0;
            isVertical = vertical;
        }
        public bool isVertical;
        public bool isHorizontal => !isVertical;
        public Vector2 initialLocation;
        public Vector2 nextControlRenderingLocation;
        /// <summary>
        /// current widest point of the UI.
        /// </summary>
        public int width
        {
            get => Math.Max(Math.Max(minimumWidth, spectulativeWidth), (int)Math.Abs(nextControlRenderingLocation.X - initialLocation.X));
            set { speculateWidth(value); }
        }
        public void speculateWidth(int value)
        {
            if (value > spectulativeWidth)
                spectulativeWidth = value;
        }
        private int spectulativeWidth;
        /// <summary>
        /// Minimum width the UI can be.
        /// if -1, UI can be 0 pixels wide.
        /// </summary>
        public int minimumWidth;
        public int minimumHeight;
        /// <summary>
        /// Height of the window.
        /// </summary>
        public int height
        {
            get => Math.Max(Math.Max(minimumHeight, spectulativeHeight), (int)Math.Abs(initialLocation.Y - nextControlRenderingLocation.Y));
            set { speculateHeight(value); }
        }
        public void speculateHeight(int value)
        {
            if (value > spectulativeHeight)
                spectulativeHeight = value;
        }
        private int spectulativeHeight;
    }
}
