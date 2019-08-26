using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI
{
    class UIElementClickEventArgs : EventArgs
    {
        public bool isControllerClick;
        public Rectangle mouseLocation;
        public UIElementClickEventArgs(Rectangle MouseLocation, bool IsControllerClick)
        {
            isControllerClick = IsControllerClick;
            mouseLocation = MouseLocation;
        }
    }
}
