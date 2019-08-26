using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    internal class Particle : IDisposable, IEquatable<Particle>
    {

        public static List<Particle> active = new List<Particle>();
        private static List<Particle> purgeList = new List<Particle>();
        public static void purge()
        {
            foreach(Particle p in purgeList)
            {
                p.Dispose();
            }
            purgeList.Clear();
        }
        private static Random generator = new Random();
        public static Texture2D particle_sprites;
        public const int dim = 40;
        /// <summary>
        /// global location of particle (global systems not yet implemented.)
        /// </summary>
        public Vector2 location = new Vector2();
        public byte type;
        public Color color;
        /// <summary>
        /// Time to live, in frames.
        /// </summary>
        int timeToLive;
        public Particle(byte Type, Rectangle within, Color? Colour = null, int TimeToLive = 300)
        {
            type = Type;
            location = new Vector2(generator.Next(within.X, within.X + within.Width), generator.Next(within.Y, within.Y + within.Height));
            color = Colour ?? Color.White;
            timeToLive = TimeToLive;
            active.Add(this);
        }
        public Particle(byte Type, int XLocation, int YLocation, Color? Colour = null, int TimeToLive = 300)
        {
            type = Type;
            color = Colour ?? Color.White;
            location = new Vector2(XLocation, YLocation);
            timeToLive = TimeToLive;
            active.Add(this);
        }
        public Particle(byte Type, Point Location, Color? Colour = null, int TimeToLive = 300)
        {
            type = Type;
            color = Colour ?? Color.White;
            location = new Vector2(Location.X, Location.Y);
            timeToLive = TimeToLive;
            active.Add(this);
        }
        public Particle(byte Type, Vector2 Location, Color? Colour = null, int TimeToLive = 300)
        {
            type = Type;
            color = Colour ?? Color.White;
            location = Location;
            timeToLive = TimeToLive;
            active.Add(this);
        }

        public void Dispose()
        {
            active.Remove(this);
        }


        public bool Equals(Particle other)
        {
            return other.color == color && other.type == type;
        }

        public void update(GameTime gameTime)
        {
            timeToLive -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeToLive <= 0)
            {
                purgeList.Add(this);
            }
            if (timeToLive <= 30)
            {
                color *= .7f;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(particle_sprites, location - Camera.location, new Rectangle(0, type * dim, dim, dim), color, 0f, new Vector2(dim / 2, dim / 2), .25f, SpriteEffects.None, 0f);
        }
    }
}
