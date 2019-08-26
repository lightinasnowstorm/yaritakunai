using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace やりたくない.世界
{
    internal partial class World
    {
        uint headerSaveFeatures = (uint)(worldSaveHeaderFeatures.name | worldSaveHeaderFeatures.widthAndHeight);

        /// <summary>
        /// loads the entire world in one go
        /// </summary>
        public static World load(string FileName)
        {
            World world = new World
            {
                fileName = FileName
            };
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(FileName)))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    //get the features that the save file supports.
                    uint featuresRaw = reader.ReadUInt32();
                    //this is a waste compared with (header&feature)!=0
                    bool[] features = bitsBoolsLE(featuresRaw);
                    if (features[logbase((int)worldSaveHeaderFeatures.name,2)])
                    {
                        world.name = reader.ReadString();
                    }
                }
            }
            return world;
        }
        /*public void chunkedLoad()
        {

        }*/
        public void loadChunk()
        {

        }
        public void save()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    //write stuff here.
                    writer.Write(headerSaveFeatures);
                }
                File.WriteAllBytes(fileName, stream.ToArray());
            }

        }

        /// <summary>
        /// returns a bool[] of the content of bits with big endian bit ordering
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static bool[] bitsBoolsBE(uint bits)
        {
            bool[] ret = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                ret[i] = (bits & 0x80_00_00_00) == 0x80_00_00_00;
                bits <<= 1;
            }
            return ret;
        }
        /// <summary>
        /// returns a bool[] of the content of bits with little endian bit ordering
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static bool[] bitsBoolsLE(uint bits)
        {
            bool[] ret = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                ret[i] = (bits & 1) == 1;
                bits >>= 1;
            }
            return ret;
        }
        static int logbase(int a, int b) => (int)(Math.Log(a) / Math.Log(b));
    }
    enum worldSaveHeaderFeatures : uint
    {
        name = 0b1,
        widthAndHeight = 0b10

    }
}
