using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace やりたくない
{
    internal static class RenderTargetManager
    {
        public static GraphicsDevice device;
        public static void init(GraphicsDevice Device)
        {
            device = Device;

        }
        public static void sizeRenderTargets()
        {
            //Cannot do anything if device is null.
            if (device == null)
                return;

            int targetWidth = Main.zeroedWindow.Width;
            int targetHeight = Main.zeroedWindow.Height;
            //set render targets to correct size.
            transitionBeforeFrame = new RenderTarget2D(device, targetWidth, targetHeight);
            transitionAfterFrame = new RenderTarget2D(device, targetWidth, targetHeight);
            framePreRender = new RenderTarget2D(device, targetWidth, targetHeight);
        }

        public static void setRenderTarget(RenderTarget2D target)
        {
            renderTargetStack.AddFirst(target);
            device.SetRenderTarget(target);
        }
        public static void unsetRenderTarget()
        {

            if (renderTargetStack.Count < 2)
            {
                renderTargetStack.RemoveFirst();
                device.SetRenderTarget(null);
            }
            else
            {
                renderTargetStack.RemoveFirst();
                device.SetRenderTarget(renderTargetStack.First());
            }
        }
        //We have render targets to render to
        #region render targets
        public static RenderTarget2D transitionBeforeFrame;
        public static RenderTarget2D transitionAfterFrame;
        public static RenderTarget2D framePreRender;
        #endregion

        private static LinkedList<RenderTarget2D> renderTargetStack = new LinkedList<RenderTarget2D>();
    }
}
