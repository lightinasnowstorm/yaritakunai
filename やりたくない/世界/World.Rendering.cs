using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using やりたくない;

namespace やりたくない.世界
{
    internal partial class World
    {
        public void draw(SpriteBatch spriteBatch)
        {
            //figure out where it is in the world: camera is top-left of window, (0,0) is top-left of world
            Vector2 camblockpos = Camera.location / blockSizePx;
            //this is what I imagine camblockpos-- would look like
            camblockpos.X--; camblockpos.Y--;
            //round the position doooown.
            camblockpos.X = (int)camblockpos.X;
            camblockpos.Y = (int)camblockpos.Y;
            //multiply by blockSizePx to get the pixel position of the block
            Vector2 topLeftBlockPosPixels = camblockpos * blockSizePx;
            //sub the camera position to get the screen location
            Vector2 startDrawPos = topLeftBlockPosPixels - Camera.location;
            int xScreenWidthBlocks = Main.zeroedWindow.Width / blockSizePx + 1;
            int yScreenHeightBlocks = Main.zeroedWindow.Height / blockSizePx + 1;
            //double for loops.
            for (int xPixelCounter = (int)startDrawPos.X, xBlockCounter = (int)camblockpos.X;
                xBlockCounter <= (int)camblockpos.X + xScreenWidthBlocks;
                xPixelCounter += blockSizePx, xBlockCounter++)
            {
                for (int yPixelCounter = (int)startDrawPos.Y, yBlockCounter = (int)camblockpos.Y;
                    yBlockCounter <= (int)camblockpos.Y + yScreenHeightBlocks;
                    yPixelCounter += blockSizePx, yBlockCounter++)
                {
                    //build a byte out of the blocks around it
                    //how to do this efficiently?

                    spriteBatch.Draw(Block.blockTextures[GetBlock(xBlockCounter, yBlockCounter).Type], new Vector2(xPixelCounter, yPixelCounter), null/*this has to be figured out*/, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f/*need an actual layer for it (I think it's immediate draw so that's a problem but like not*/);
                }
            }
        }
    }
}
