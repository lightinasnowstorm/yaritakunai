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
    /// Holds the in-game UI
    /// </summary>
    static class IGUI
    {
        internal enum IGUIElements
        {
            playerHealthbar = 0,
            NPCHealthbars = 1
        }
        private static SpriteBatch spriteBatch = Main.spriteBatch;
        public static bool[] shownElements = new bool[] { true, true };
        public static void draw()
        {
            if (shownElements[(int)IGUIElements.playerHealthbar])
            {
                drawPlayerHealthbar();
            }
            if (shownElements[(int)IGUIElements.NPCHealthbars])
            {
                drawNPCHealthbars();
            }
        }
        public static void drawPlayerHealthbar()
        {
            spriteBatch.Draw(Main.notverymagicpixel, new Rectangle(30, 30, 100, 10), new Color(0x62, 0x58, 0x55));
            spriteBatch.Draw(Main.notverymagicpixel, new Rectangle(30, 30, Main.currentPlayer.health, 10), new Color(0xe1, 0xb3, 0xaa));
        }
        public static void drawNPCHealthbars()
        {
            foreach (NPC npc in NPC.Active)
            {
                Rectangle texrect = NPC.npcSprites[(int)npc.type].Bounds;
                //Draw NPC healthbar if it has <max hp (or greater?)
                if (/*true || */npc.health != npc.maxHealth)
                {
                    //draw hp bar using supermagic pixels- gray for missing, coloured depending on amount left.


                    //height of HP bar
                    int hpBarHeight = 7;

                    //Width of HP bar. The bar is automatically centered.
                    int hpBarWidth = 70;
                    //Location of the HP bar- center of the bottom of the NPC.
                    Vector2 hpLocation = new Vector2((int)npc.location.X - (int)Camera.location.X + (texrect.Width / 2), (int)npc.location.Y - (int)Camera.location.Y + texrect.Height);

                    //Vertical transformtion of the HP bar.
                    int hpBarYChange = -2;
                    //vertical transformation for HP string
                    int hpStringYChange = 3;

                    //Get the % max health.
                    float healthpercent = (float)npc.health / (float)npc.maxHealth;
                    //get a colour based on % max health
                    Color basedOnHP = new Color((int)(255f - healthpercent * 255f), (int)(healthpercent * 255f), (int)0);
                    //Colour for the background of the HP bar.
                    Color backgroundColour = new Color(0x62, 0x58, 0x55);
                    //Color for string.
                    Color stringColour = Color.White;

                    //Draw background for the HP bar.
                    Main.spriteBatch.Draw(Main.notverymagicpixel, new Rectangle((int)hpLocation.X - hpBarWidth / 2, (int)hpLocation.Y + hpBarYChange, hpBarWidth, hpBarHeight), backgroundColour);

                    //Draw the HP in the same location as the background.
                    Main.spriteBatch.Draw(Main.notverymagicpixel, new Rectangle((int)hpLocation.X - hpBarWidth / 2, (int)hpLocation.Y + hpBarYChange, (int)((int)hpBarWidth * healthpercent), hpBarHeight), basedOnHP);
                    //Draw amount of HP
                    string HPString = $"{npc.health}/{npc.maxHealth}";
                    Main.spriteBatch.DrawString(Main.healthFont, HPString, new Vector2(hpLocation.X - (Main.healthFont.MeasureString(HPString).X / 2), hpLocation.Y + hpStringYChange), stringColour);
                }
            }
        }

    }
}
