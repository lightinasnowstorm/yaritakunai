//#define FORCE_LOAD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using やりたくない.PlayerControllers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace やりたくない
{
    [Serializable]
    internal class Player : IControllable, IAnimationControllable, IPhysicsControllable
    {

        public string Name
        {
            get => name;
            set { name = value; }
        }
        string name;

        public Player(Vector2 Location) : this(null, NoController.noController, Location)
        {

        }


        public Player(string Name, Controller controller, Vector2 Location)
        {
            name = Name;
            selfController = controller;
            controller.Controlled = this;//add self to controller
            location = Location;
            animationController = new PlayerAnimationController(this);
            physicsController = new PhysicsController(this);
            //make inventory
            createInventoryHolders();
            save();
        }

        static byte[] shebangLine = Encoding.Default.GetBytes(@"#!/usr/bin/env mono /opt/yaritakunai/やりたくない.exe
");
        static void throwAwayShebangLine(FileStream stream)
        {
            try
            {
                stream.Read(shebangLine, 0, shebangLine.Length);
            }
            catch { }
        }

        public void save()
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = FileHelper.GetStream(Settings.playerSavesLocation + name + ".ghb", FileMode.Create, FileAccess.Write, FileShare.None);
            if (stream == null)
                return;
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(shebangLine);
            writer.Write(name);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        /// <summary>
        /// only can load local players; therefore the controller is automatically created for locally
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Player load(string path)
        {
            FileStream stream = FileHelper.GetStream(path, FileMode.Open, FileAccess.Read);
            if (stream == null)
                return null;

            //throw away shebang
            throwAwayShebangLine(stream);

            string playerName = playerNameFromStream(stream);
            Player player = playerFromStream(stream);

            stream.Close();

            if (playerName == null || player == null)
                return null;

            player.selfController = new PlayerInputController(player);
            player.animationController = new PlayerAnimationController(player);
            player.physicsController = new PhysicsController(player);
            return player;
        }

        public static string playerNameFromFile(string path)
        {
            FileStream stream = FileHelper.GetStream(path, FileMode.Open, FileAccess.Read);
            if (stream == null)
                return null;

            //throw away shebang
            throwAwayShebangLine(stream);

            return playerNameFromStream(stream);
        }

        private static string playerNameFromStream(FileStream stream)
        {
#if !FORCE_LOAD
            try
            {
#endif
                BinaryReader reader = new BinaryReader(stream);
                return reader.ReadString();
#if !FORCE_LOAD
            }
            catch
            {
                return null;
            }
#endif
        }

        private static Player playerFromStream(FileStream stream)
        {
#if !FORCE_LOAD
            try
            {
#endif
                IFormatter formatter = new BinaryFormatter();
                return (Player)formatter.Deserialize(stream);
#if !FORCE_LOAD
            }
            catch
            {
                return null;
            }
#endif
        }

        //inventory
        public const int inventorySlots = 60;

        public Holder<Item>[] inventory = new Holder<Item>[inventorySlots];

        private void createInventoryHolders()
        {
            for (int i = 0; i < inventorySlots; i++)
            {
                inventory[i] = new Holder<Item>(null);
            }
        }


        //sprite and that stuff is constant
        public static Texture2D spriteSheet;
        public static Rectangle relativehitbox = new Rectangle(10, 8, 38, 87);
        public Rectangle Hitbox => relativehitbox;

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, location - Camera.location, spriteSheet.Bounds, Color.White, rotation, center, 1f, effects, 0f);
        }


        static Animation[] animations = new Animation[2];
        public Animation[] Animations
        {
            get => animations;
        }

        uint currentAnimation = 0;
        public uint CurrentAnimation
        {
            get => currentAnimation;
            set { currentAnimation = value; }
        }

        public uint CurrentFrame => animations[currentAnimation].currentFrame;

        float rotation = 0;
        public float Rotation
        {
            get => rotation;
            set { rotation = value; }
        }
        [NonSerialized]
        Vector2 scale;
        public Vector2 Scale
        {
            get => scale;
            set { scale = value; }
        }
        public float ScaleX
        {
            get => scale.X;
            set { scale.X = value; }
        }
        public float ScaleY
        {
            get => scale.Y;
            set { scale.Y = value; }
        }
        [NonSerialized]
        SpriteEffects effects = SpriteEffects.None;
        public SpriteEffects Effects
        {
            get => effects;
            set { effects = value; }
        }


        [NonSerialized]
        Vector2 center = new Vector2(28, 60);
        //Members for IControllable
        [NonSerialized]
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

        //IPhysicsControllable
        //could have rectangle that's updated with position for actual hitbox.
        //Rectangle realHitbox = new Rectangle(0, 0, relativehitbox.Width, relativehitbox.Height);
        public Rectangle ActualHitbox => new Rectangle((int)(Location.X + relativehitbox.X), (int)(Location.Y + relativehitbox.Y), relativehitbox.Width, relativehitbox.Height);
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
        [NonSerialized]
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set { velocity = value; }
        }
        public bool falls => true;
        public bool terrainColliding => true;

        //health also is a component, but i think that's like direct inheretence from Entity or something
        public int health = 100;
        public int maxHealth = 100;

        public static int msInvincibility = 500;
        [NonSerialized]
        public int currentMSInvincibility;

        /// <summary>
        /// this sounds like LoL's Zoe but it's really just a particle test and can be removed.
        /// </summary>
        public void bubble()
        {
            new Particle(0, ActualHitbox, Color.LightBlue, 200);
        }
        public void vbubble()
        {
            new Particle(0, ActualHitbox, Color.Green, 176);
        }

        /// <summary>
        /// Controller for player's movement.
        /// </summary>
        [NonSerialized]
        Controller selfController;
        /// <summary>
        /// Controls animation of the player
        /// </summary>\
        [NonSerialized]
        AnimationController animationController;
        /// <summary>
        /// Controls the player's physics.
        /// </summary>
        [NonSerialized]
        PhysicsController physicsController;

        /// <summary>
        /// The main update 
        /// </summary>
        /// <param name="gameTime"></param>
        public void update(GameTime gameTime)
        {
            //should be a component for health/invincibility/damage/etc
            //recalculate the damage timer
            if (currentMSInvincibility > 0)
            {
                currentMSInvincibility -= gameTime.ElapsedGameTime.Milliseconds;
            }
            /* Updates need to be ordered in this order:
             * movement ->
             * physics ->
             * animation ->
             * draw(?)
             * but draw might just be completely seperate
             */
            //update position
            selfController.update(gameTime);
            physicsController.update(gameTime);
            if (velocity.X != 0 && gameTime.TotalGameTime.Ticks % 3 == 0)
                bubble();
            if (velocity.Y != 0 && gameTime.TotalGameTime.Ticks % 3 == 0)
                vbubble();
            animationController.update(gameTime);
        }
        public static void init()
        {
            //what's initilazation? something for static classes. this isn't a static class, and thus, does not need to be initialized.
            //Awaiting deprecation.
            //or not i need to set the animations manually (for now anyways)
            animations[0] = new Animation(0, 0, SpriteEffects.None);
            animations[1] = new Animation(0, 0, SpriteEffects.FlipHorizontally);
        }


        /// <summary>
        /// this should be in physics but that doesn't exist yet
        /// this is comparing if the player should get hurt by projectiles
        /// </summary>
        /// <param name="p"></param>
        public void checkHitboxAndReact(Projectile p)
        {
            if (p.ActualHitbox.Intersects(ActualHitbox))
            {
                if (p.harmsPlayer)
                {
                    getHit(p.damage, p.damageType);
                    if (!p.piercing)
                        p.purgeThis();

                    //Add a onPierce() for piercing projectiles?
                }
            }
        }
        /// <summary>
        /// the player takes a hit of damage.
        /// will be from a diffent file but not yet
        /// </summary>
        /// <param name="damage"></param>
        public void getHit(int damage, DamageType type)
        {
            if (currentMSInvincibility <= 0)
            {
                health -= damage;//TODO: add damage reduction, based on damage type
                //Anti Chain Damage +0
                currentMSInvincibility = msInvincibility;
            }

            if (health < 0)
            {
                die();
            }
        }

        public void hitGround(float verticalVelocity)
        {
            if (verticalVelocity > 500)
                getHit((int)verticalVelocity / 10, DamageType.Fall);
        }
        /// <summary>
        /// player dies. currently is just a breakpoint (on my end)
        /// at the ;
        /// </summary>
        public void die()
        {
            //don't want negative health
            health = 0;

            ;
        }

    }
    enum PlayerAnimation
    {
        left,
        right
    }
}
