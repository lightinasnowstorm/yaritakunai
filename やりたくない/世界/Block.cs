using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない.世界
{
    internal struct Block
    {
        public static Texture2D[] blockTextures = new Texture2D[blockAmount];
        public const int blockAmount = 2;
        public const int size = 16;


        private ushort type;
        public ushort Type
        {
            get => type;
            set { type = value; refreshFlags(); }
        }



        public Block(ushort Type)
        {
            type = Type;
            flags = 0;
            refreshFlags();
        }

        /// <summary>
        /// flags is laid out in this order
        /// from high to low
        /// 0x80: solid: whether or not the block is solid
        /// 0x40: liquid: whether or not the block is a liquid
        /// 0x20: falling: whether or not the block falls (this bit applies to liquids as well.)
        /// 0x10: luminous (not shaded)
        /// 0x08:
        /// 0x04:
        /// 0x02:
        /// 0x01:
        /// </summary>
        byte flags;
        private void refreshFlags()
        {
            flags = 0;
            if (solidBlocks.Contains(type))
                flags |= 0x80;
            if (liquidBlocks.Contains(type))
                flags |= 0x40;
            if (fallingBlocks.Contains(type))
                flags |= 0x20;
        }

        private static HashSet<ushort> solidBlocks = new HashSet<ushort>() { 1 };
        public bool isSolid => (flags & 0x80) != 0;

        private static HashSet<ushort> liquidBlocks = new HashSet<ushort>() { };
        public bool isLiquid => (flags & 0x40) != 0;

        private static HashSet<ushort> fallingBlocks = new HashSet<ushort>() { };
        public bool isFalling => (flags & 0x20) != 0;

        public static Block emptyBlock = new Block();
    }
}
