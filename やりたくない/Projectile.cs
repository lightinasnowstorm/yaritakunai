using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace やりたくない
{
    internal class Projectile : IEquatable<Projectile>, IControllable, IPhysicsControllable
    {
        //Static members
        public const int numprojectiletypes = 3;
        public static Texture2D[] projectileTextures = new Texture2D[numprojectiletypes];

        public static List<Projectile> active = new List<Projectile>();
        private static List<Projectile> purgeList = new List<Projectile>();

        public static void init()
        {
            resetPrototypes();
        }
        public static void purge()
        {
            foreach (Projectile p in purgeList)
            {
                active.Remove(p);
            }
            purgeList.Clear();
        }

        Controller controller;
        PhysicsController physicsController;

        public projectileType type;

        Vector2 center;

        Vector2 location;
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
        public float rotation;
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
        public Rectangle Hitbox
        {
            get => hitbox;
        }
        /// <summary>
        /// get the hitbox for the projectile
        /// </summary>
        public Rectangle ActualHitbox => new Rectangle((int)(Location.X + hitbox.X), (int)(Location.Y + hitbox.Y), hitbox.Width, hitbox.Height);


        public bool harmsPlayer;
        public bool harmsEnemies;

        public bool piercing;

        //animation stuff i suppose, should add these to IAnimationControllable for reasons
        //they're in IAnimationControllable but projectile doesn't inherit that

        public float scale;

        public Color color;

        public DamageType damageType;
        public int damage;


        public SpriteEffects effects;


        private static Random projectileGenRandom = new Random();


        public Projectile(projectileType Type, Vector2 Location, Controller Controller, int? Damage = null, DamageType? DamageType = null, float Scale = 1f, float Rotation = 0f, Color? colour = null, SpriteEffects Effects = SpriteEffects.None)
        {
            type = Type;


            controller = Controller;
            controller.Controlled = this;
            physicsController = new PhysicsController(this);
            scale = Scale;
            rotation = Rotation;
            effects = Effects;
            color = colour ?? Color.White;
            location = Location;

            damage = Damage ?? prototypes[(int)type].damage;
            damageType = DamageType ?? prototypes[(int)type].damageType;

            harmsPlayer = prototypes[(int)type].harmsPlayer;
            harmsEnemies = prototypes[(int)type].harmsEnemies;
            piercing = prototypes[(int)type].piercing;
            hitbox = prototypes[(int)type].hitbox;
            center = prototypes[(int)type].center;

            active.Add(this);
        }

        public void getHit()
        {
            //can be destroyed
        }

        public void hitGround(float verticalVelocity)
        {

        }

        public bool Equals(Projectile other)
        {
            return other.harmsEnemies == harmsEnemies &&
                other.harmsPlayer == harmsPlayer &&
                other.type == type &&
                other.controller == controller &&
                other.Location == Location &&
                other.rotation == rotation;
            //location is added to stop projectiles from propagating deletion to all projectiles of a type!

        }

        public void update(GameTime gameTime)
        {
            controller.update(gameTime);
            physicsController.update(gameTime);
            offScreenChecking();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(projectileTextures[(int)type], location - Camera.location, projectileTextures[(int)type].Bounds, color, rotation, center, scale, effects, 0);
        }

        void offScreenChecking()
        {
            Vector2 windowRelativeLocation = Location - Camera.location;
            if (windowRelativeLocation.X > 3 * Main.zeroedWindow.Width + 1000 ||
                windowRelativeLocation.X < -3 * Main.zeroedWindow.Width - 1000 ||
                windowRelativeLocation.Y > 3 * Main.zeroedWindow.Height + 1000 ||
                windowRelativeLocation.Y < -3 * Main.zeroedWindow.Height - 1000)
            {
                purgeThis();
            }
        }

        public void purgeThis()
        {
            purgeList.Add(this);
        }

        private static void resetPrototypes()
        {
            string allJson = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("やりたくない.Resources.projectiles.json")).ReadToEnd();
            dynamic prototypeJson = JsonConvert.DeserializeObject(allJson);
            foreach (var prototype in prototypeJson)
            {
                int projNum = Int32.Parse(prototype.Name);
                var values = prototype.Value;
                prototypes[projNum].harmsPlayer = values.harmsPlayer;
                prototypes[projNum].harmsEnemies = values.harmsEnemies;
                prototypes[projNum].piercing = values.piercing;
                prototypes[projNum].damage = values.damage;
                prototypes[projNum].damageType = Enum.Parse(typeof(DamageType), values.damageType.ToString());
                prototypes[projNum].hitbox.X = values.hitbox.X;
                prototypes[projNum].hitbox.Y = values.hitbox.Y;
                prototypes[projNum].hitbox.Width = values.hitbox.Width;
                prototypes[projNum].hitbox.Height = values.hitbox.Height;
                prototypes[projNum].center.X = values.center.X;
                prototypes[projNum].center.Y = values.center.Y;
            }
        }

        private static projectilePrototype[] prototypes = new projectilePrototype[numprojectiletypes];
    }

    internal struct projectilePrototype
    {
        public bool harmsPlayer;
        public bool harmsEnemies;
        public bool piercing;
        public int damage;
        public DamageType damageType;
        public Rectangle hitbox;
        public Vector2 center;

        public projectilePrototype(bool HarmsPlayer, bool HarmsEnemies, bool Piercing, int Damage, DamageType DamageType, Rectangle Hitbox, Vector2 Center)
        {
            harmsPlayer = HarmsPlayer;
            harmsEnemies = HarmsEnemies;
            piercing = Piercing;
            damage = Damage;
            damageType = DamageType;
            hitbox = Hitbox;
            center = Center;
        }
    }

    enum projectileType
    {
        someMagicalThing,
        emilHead,
        blizzardPiece
    }
}
