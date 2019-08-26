using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace やりたくない.世界
{
    internal class Chunk
    {
        public Chunk()
        {
            blocks = new Block[size, size];
            for(int i=0; i<size; i++)
            {
                for(int j=0;j<size;j++)
                {
                    blocks[i, j] = new Block();
                }
            }
        }
        public const int size = 16;
        public Block[,] blocks;
    }
}
