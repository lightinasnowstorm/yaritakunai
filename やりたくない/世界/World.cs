using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.世界
{
    /// <summary>
    /// A world will act as a contiguous Block[,], abstracting over the chunks that make it up.
    /// </summary>
    internal partial class World
    {
        const int blockSizePx = Block.size;
        const int chunkSize = Chunk.size;
        private Chunk[,] chunks;
        public int width, height;


        string name;
        string fileName;

        public Block GetBlock(int x, int y)
        {
            //Any out of bounds blocks return an empty block.
            if (x < 0 || x >= width || y < 0 || y >= height)
                return Block.emptyBlock;
            return chunks[x / chunkSize, y / chunkSize].blocks[x % chunkSize, y % chunkSize];
        }

        public void SetBlockType(int x, int y, ushort value)
        {
            if (x < 0 || x > width || y < 0 || y > height)
                return;//ignore if it's outside bounds.
            chunks[x / chunkSize, y / chunkSize].blocks[x % chunkSize, y % chunkSize].Type = value;
        }

        public void SetBlock(int x, int y, Block value)
        {
            if (x < 0 || x > width || y < 0 || y > height)
                return;//ignore if it's outside bounds.
            chunks[x / chunkSize, y / chunkSize].blocks[x % chunkSize, y % chunkSize] = value;
        }


        public bool isSolidBlockInRectangle(Rectangle rectang)
        {
            //check if there is solid blocks in the rectangle.
            //do the thing with the starting point
            //the starting point is the block within which the rectang location vector あります。
            int startX = rectang.X / blockSizePx;
            int startY = rectang.Y / blockSizePx;
            int endX = startX + (int)Math.Ceiling(rectang.Width / (float)blockSizePx);//Block position is rounded down by the /blockSizePx, so ceiling it.
            int endY = startY + (int)Math.Ceiling(rectang.Height / (float)blockSizePx); ;//Same as above.
            //Double for loops.
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    //Any solid block is inside, is inside.
                    //Do not then search more.
                    if (GetBlock(x, y).isSolid)
                        return true;
                }
            }
            return false;
        }

    }
}
