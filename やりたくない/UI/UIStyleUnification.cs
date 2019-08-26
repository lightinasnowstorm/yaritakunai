using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI
{
    internal static class UIStyleUnification
    {
        private static colorScheme scheme = new colorScheme()
        {
            titlebarColor = Color.LightGray,
            windowBackgroundColor = new Color(0x2B, 0x2B, 0x2B),
            errorLineColor = Color.Red,
            elementBackground = new Color(0x55, 0x55, 0x55),
            borderColor = Color.Black,
            interactBorder = Color.Magenta,
            interactBackground = Color.DarkMagenta,
            interactHoverBackground = new Color(0xF5, 0x3C, 0xCE),
            selectOutline = Color.Aqua,
            elementSelect = Color.Yellow,
            defaultText = Color.White,
            fadedText = Color.LightGray,
            windowBorderColor = Color.White,
            windowBorderColorFocused = new Color(0xFF, 0x7F, 0xD8)
        };
        //override line style
        public const int errorLineHeight = 15;
        public const int windowBorder = 1;
        //window styling
        public static Color titlebarColor => scheme.titlebarColor;
        //2b is best ~~android~~ color
        public static Color windowBackgroundColor => scheme.windowBackgroundColor;
        public static Color windowBorderColor => scheme.windowBorderColor;
        public static Color windowBorderColorFocused => scheme.windowBorderColorFocused;
        public static Color errorLineColor => scheme.errorLineColor;
        //element styling
        public const int borderSize = border + padding;
        public const int border = 1;
        public const int padding = 2;
        public static Color elementBackground => scheme.elementBackground;
        public static Color borderColor => scheme.borderColor;
        public static Color interactBorder => scheme.interactBorder;
        public static Color interactBackground => scheme.interactBackground;
        public static Color interactHoverBackground => scheme.interactHoverBackground;
        public static Color selectOutline => scheme.selectOutline;
        public static Color elementSelect => scheme.elementSelect;
        public static Color defaultText => scheme.defaultText;
        public static Color fadedText => scheme.fadedText;
    }
    internal class colorScheme
    {
        public Color titlebarColor, windowBackgroundColor, errorLineColor, windowBorderColor, windowBorderColorFocused;
        public Color elementBackground, borderColor, interactBorder, interactBackground, interactHoverBackground, selectOutline, elementSelect, defaultText, fadedText;
    }
}
