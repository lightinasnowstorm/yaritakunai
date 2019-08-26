using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace やりたくない
{
    internal static class Transitions
    {
        public static int transitionTimeMS = 500;

        public static Transition current;

        private static screens transitionTarget;

        private static Main transitioningMain;

        private static bool setupCompleted = false;

        private static Texture2D beforeFrame;
        private static Texture2D afterFrame;

        public static void SetupAndBegin(GraphicsDevice device, Main main, Transition transition, screens from, screens to)
        {
            Prepare(device, main, transition, from, to);
            Begin();
        }

        public static void Prepare(GraphicsDevice device, Main main, Transition transition, screens from, screens to)
        {
            //Cannot go to or from a transitioning screen with a transition.
            if (from == screens.transitioning || to == screens.transitioning)
                return;

            transitionTarget = to;
            transitioningMain = main;

            current = transition;
            current.currentMSTime = 0;


            main.drawFrameToRenderTarget(from, RenderTargetManager.transitionBeforeFrame);
            main.drawFrameToRenderTarget(to, RenderTargetManager.transitionAfterFrame);
            //Save the render targets
            beforeFrame = RenderTargetManager.transitionBeforeFrame;
            afterFrame = RenderTargetManager.transitionAfterFrame;

            setupCompleted = true;
        }

        public static void Begin()
        {
            if (!setupCompleted)
                return;
            transitioningMain.switchScreenTo(screens.transitioning);
            setupCompleted = false;
        }

        public static void update(GameTime gameTime)
        {
            current.currentMSTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (current.currentMSTime > transitionTimeMS || Input.mouseMainButton)//Click to skip.
                transitioningMain.switchScreenTo(transitionTarget);
        }

        public static fade Fade = new fade();
        internal class fade : Transition
        {
            public override void draw(SpriteBatch spriteBatch, GameTime gameTime)
            {
                //draw fading.
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                float percentFinished = (float)currentMSTime / transitionTimeMS;
                //Clamp to 100% finished.
                percentFinished = Math.Min(percentFinished, 1f);
                uint up = (uint)(255 - percentFinished * 255);
                Color fadingOutFrameColour = new Color((byte)up, (byte)up, (byte)up, (byte)up);
                spriteBatch.Draw(beforeFrame, Main.zeroedWindow, fadingOutFrameColour);
                //draw future screen, faded with negative time passed.
                uint down = (uint)(percentFinished * 255);
                Color fadingInFrameColour = new Color((byte)down, (byte)down, (byte)down, (byte)down);
                spriteBatch.Draw(afterFrame, Main.zeroedWindow, fadingInFrameColour);
                spriteBatch.End();
            }
        }
        internal abstract class Transition
        {
            public int currentMSTime = 0;
            public abstract void draw(SpriteBatch spriteBatch, GameTime gameTime);
        }
    }

}
