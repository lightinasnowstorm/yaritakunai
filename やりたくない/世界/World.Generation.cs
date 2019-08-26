using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace やりたくない.世界
{
    internal partial class World
    {
        /// <summary>
        /// empty initialization. For doing things with worlds. Like loading.
        /// </summary>
        public World()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width">width of the world</param>
        /// <param name="Height">height of the world</param>
        public World(int Width, int Height)
        {
            //create a world and split into chunks.
            //round down width and height to chunk sized ones.
            width = Width - Width % chunkSize;
            height = Height - Height % chunkSize;
            chunks = new Chunk[width / chunkSize, height / chunkSize];
            //initialize chunks
            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {
                    chunks[i, j] = new Chunk();
                }
            }
            ;
        }
        public static World generateWorld(int Width, int Height)
        {
            //no worldgen created yet, but give a world.
            World result = new World(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    result.SetBlockType(i, j, (ushort)((j > 20 && i % 10 <7 || j>80) && (j<55 || j>80) ? 1 : 0));
                }

            }
            return result;
        }
    }
}
