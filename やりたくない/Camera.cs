using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない
{
    internal static class Camera
    {
        public static Vector2 location = Vector2.Zero;
        public static void init()
        {

        }
        public static void update(GameTime gametime)
        {
            //this is bad
            location = Main.currentPlayer.Location - Main.center;
        }
    }
}
