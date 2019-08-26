using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace やりたくない
{
    internal partial class NPC : IDisposable, IControllable, IPhysicsControllable
    {
        //const members and type declaration
        const int numNpcTypes = 1;

        public static Texture2D[] npcSprites = new Texture2D[numNpcTypes];

        public enum npcTypes
        {
            clone = 0
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle texrect = npcSprites[(int)type].Bounds;
            spriteBatch.Draw(npcSprites[(int)type], location - Camera.location, null/*add frame later*/, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
        }


        //Static members
        public static List<NPC> Active = new List<NPC>();
        public static List<NPC> purgeList = new List<NPC>();

        //Static methods
        public static void purge()
        {
            foreach (NPC n in purgeList)
            {
                n.Dispose();
            }
            purgeList.Clear();
        }


        //Instance variables
        public npcTypes type;
        Vector2 center;
        public Vector2 location;
        public Vector2 Location
        {
            get => location - center;
            set { location = value + center; }
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
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set { velocity = value; }
        }
        private float rotation;
        public float Rotation
        {
            get => rotation;
            set { rotation = value; }
        }


        bool thisFalls = false;
        public bool falls => thisFalls;

        bool thisTerrainCollides = false;
        public bool terrainColliding => thisTerrainCollides;

        Rectangle hitbox;
        public Rectangle Hitbox => hitbox;
        public Rectangle ActualHitbox => new Rectangle((int)Location.X + hitbox.X, (int)Location.Y + hitbox.Y, hitbox.Width, hitbox.Height);

        public int health;
        public int maxHealth;

        /// <summary>
        /// invincibility frames
        /// </summary>
        public static int msInvincibility = 500;
        public int currentMSInvincibility = msInvincibility;

        Controller controller;
        PhysicsController physicsController;

        /// <summary>
        /// New constructor using controllers
        /// </summary>
        public NPC(Vector2 Location, npcTypes _Type, Controller Controller)
        {
            type = _Type;
            controller = Controller;
            controller.Controlled = this;
            physicsController = new PhysicsController(this);

            switch (type)
            {
                case npcTypes.clone:
                    health = 500;
                    maxHealth = 500;
                    hitbox = new Rectangle(10, 8, 38, 87);
                    center = new Vector2(28, 60);
                    thisFalls = true;
                    break;
            }
            //Add completed NPC to the NPCs that are currently active
            Active.Add(this);
        }


        public void update(GameTime gameTime)
        {
            if (currentMSInvincibility > 0)
            {
                currentMSInvincibility -= gameTime.ElapsedGameTime.Milliseconds;
            }
            controller.update(gameTime);
            physicsController.update(gameTime);
        }

        public void checkHitboxAndReact(Projectile projectile)
        {
            if (projectile.ActualHitbox.Intersects(ActualHitbox))
            {
                if (projectile.harmsEnemies)
                {
                    getHit(projectile.damage, projectile.damageType);
                    if (!projectile.piercing)
                        projectile.purgeThis();
                }
            }
        }



        public void getHit(int damage, DamageType type)
        {
            if (currentMSInvincibility <= 0)
            {
                health -= damage;//TODO: add damage reduction, depending on what damage
                //Anti Chain Damage +0
                currentMSInvincibility = msInvincibility;
            }

            if (health <= 0)
            {
                die();

            }
        }

        public void hitGround(float verticalVelocity)
        {
            //do nothing
        }

        static Random rand = new Random();

        public void die()
        {
            purgeList.Add(this);
            for (int i = 0; i < 20; i++)
            {
                new Particle(0, ActualHitbox, Color.Red, rand.Next(100, 250));
            }
        }

        public void Dispose()
        {
            Active.Remove(this);
        }
    }
}
