using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    internal static class DebugRenderer
    {

        private static Color[] debugColors = new Color[] { Color.Yellow, Color.White, Color.MediumAquamarine, Color.Aquamarine, Color.BlueViolet, Color.Cyan, new Color(0xff8a4fffu), new Color(0xff5350ffu) };
        public static void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            //sample string
            //spriteBatch.DrawString(font, "「スキ」なものはなんですか？", new Vector2(300, 300), debugColors[0]);
            drawSingleString(spriteBatch, 40, font, "Debug strings.", debugColors[1]);
            drawSingleString(spriteBatch, 50, font, (1000f / Main.lastUpdateTime.ElapsedGameTime.TotalMilliseconds).ToString("n1") + " UPS, 前のアップデートは" + Main.lastUpdateTime.ElapsedGameTime.TotalMilliseconds.ToString("n2") + "msでした", debugColors[2]);
            drawSingleString(spriteBatch, 80, font, (1000f / Main.lastDrawTime.ElapsedGameTime.TotalMilliseconds).ToString("n1") + " FPS, 前のフレームは" + Main.lastDrawTime.ElapsedGameTime.TotalMilliseconds.ToString("n2") + "msでした", debugColors[3]);
            drawSingleString(spriteBatch, 170, font, NPC.Active.Count + "つNPCがいる", debugColors[4]);
            drawSingleString(spriteBatch, 190, font, Projectile.active.Count + "つ飛翔体がある", debugColors[5]);
            drawSingleString(spriteBatch, 200, font, "プレイヤーは速度が{X:"+Main.currentPlayer.HorizontalVelocity.ToString("n1")+" Y:"+Main.currentPlayer.VerticalVelocity.ToString("n1")+"}です。", debugColors[6]);
            drawSingleString(spriteBatch, 220, font, "プレイヤーは{X:" + Main.currentPlayer.Location.X.ToString("n2") + " Y:" + Main.currentPlayer.Location.Y.ToString("n2") + "}にいます", debugColors[7]);
            spriteBatch.End();
        }
        private static void drawSingleString(SpriteBatch spriteBatch, int y, SpriteFont font, string drawing, Color color)
        {
            //rotated is shorter, but i don't want to do trig every frame
            int length = (int)font.MeasureString(drawing).X;
            int drawingPosition = Main.zeroedWindow.Right - length;
            spriteBatch.DrawString(font, drawing, new Vector2(drawingPosition, y), color, (float)Math.PI / 12, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
