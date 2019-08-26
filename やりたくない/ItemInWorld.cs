using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    internal class ItemInWorld : IPhysicsControllable, IDisposable
    {
        private static Random generator = new Random();

        public static List<ItemInWorld> active = new List<ItemInWorld>();
        private static List<ItemInWorld> purgeList = new List<ItemInWorld>();
        public static void purge()
        {
            foreach (ItemInWorld i in purgeList)
            {
                i.Dispose();
            }
            purgeList.Clear();
        }


        Item item;
        /// <summary>
        /// flags is laid out in this order
        /// from high to low
        /// 0x80: Large: whether or not the item is large
        /// 0x40:
        /// 0x20:
        /// 0x10:
        /// 0x08:
        /// 0x04:
        /// 0x02:
        /// 0x01:
        /// </summary>
        byte flags;

        public ItemInWorld(Item Item, Vector2 Location)
        {
            item = Item;
            location = Location;
            velocity = Vector2.Zero;
            //have to assign controller before can use 'this', so 
            controller = new PhysicsController(this);
            setFlags();
            active.Add(this);
        }
        public ItemInWorld(Item Item, Rectangle within)
        {
            velocity = Vector2.Zero;
            item = Item;
            location = location = new Vector2(generator.Next(within.X, within.X + within.Width), generator.Next(within.Y, within.Y + within.Height));
            setFlags();
            active.Add(this);
        }
        private void setFlags()
        {
            flags = 0;
        }

        PhysicsController controller;
        public void update(GameTime gameTime)
        {
            controller.update(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle drawLocation = new Rectangle((int)(location.X - Camera.location.X), (int)(location.Y - Camera.location.Y), Hitbox.Width, Hitbox.Height);
            spriteBatch.Draw(Item.itemTextures[item.id], drawLocation, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.01f);
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set { velocity = value; }
        }
        public float HorizontalVelocity
        {
            get => velocity.X;
            set { velocity.X = value; }
        }
        public float VerticalVelocity
        {
            get => velocity.Y;
            set { velocity.Y = value; }
        }

        Vector2 location;
        public Vector2 Location
        {
            get => location;
            set { location = value; }
        }
        public float LocationXComponent
        {
            get => location.X;
            set { location.X = value; }
        }
        public float LocationYComponent
        {
            get => location.Y;
            set { location.Y = value; }
        }

        //items are small unless otherwise stated
        private static HashSet<Item> largeItems = new HashSet<Item>() { };

        private static Rectangle universalSmallItemHitbox = new Rectangle(0, 0, 32, 32);
        private static Rectangle universalLargeItemHitbox = new Rectangle(0, 0, 64, 64);

        public Rectangle Hitbox => (flags & 0x80) != 0 ? universalLargeItemHitbox : universalSmallItemHitbox;

        public Rectangle ActualHitbox => new Rectangle((int)location.X, (int)location.Y, Hitbox.Width, Hitbox.Height);


        public bool falls => true;//items fall

        public bool terrainColliding => true;//items collect on floor

        public void hitGround(float verticalVelocity) { }

        public void Dispose()
        {
            active.Remove(this);
        }
    }
}
