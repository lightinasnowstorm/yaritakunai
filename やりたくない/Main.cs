//my life, 大きい願いで

//Debug preprocess variables.
//#define FORCE_RESET_SETTINGS

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using やりたくない.UI;
using やりたくない.PlayerControllers;
using やりたくない.NPCControllers;
using やりたくない.ProjectileControllers;
using やりたくない.世界;
using System.IO;

namespace やりたくない
{
    /// <summary>
    /// This is the main game.
    /// </summary>
    internal class Main : Game
    {
        public static Main refmain;

        public WindowManager gameWM;
        public WindowManager settingsWM;
        public WindowManager playerCreationWM;

        const uint maxPlayers = 256;
        static Player[] players = new Player[maxPlayers];
        static ushort thisPlayer = 0;
        public static Player currentPlayer => players[thisPlayer];
        public event yaritakunaiEventHandler localPlayerLoaded = delegate { };

        public void setPlayer(Player player)
        {
            players[thisPlayer] = player;
            localPlayerLoaded();
        }

        public static Random rand = new Random();
        public static bool isCursorDrawn = true;
        public static bool isPsuedoCursorDrawn = true;
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        /// <summary>
        /// Zeroed version of the window to perform calculations on.
        /// </summary>
        public static Rectangle zeroedWindow;
        /// <summary>
        /// Center of the window
        /// </summary>
        public static Vector2 center;
        public static screens currentScreen = screens.langSelect;

        public static World MainWorld => world;
        static World world;


        public Item cursorItem;
        public Item pseudoCursorItem;

        public Main()
        {
            refmain = this;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 600,
                PreferredBackBufferWidth = 1300
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.AllowUserResizing = true;
            world = World.generateWorld(256000, 256);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Add methods to the size change
            Window.ClientSizeChanged += updateBoundsOnChange;

            //temporary, add the player.
            //players[thisPlayer] = Player.load("player.ghb") ?? new Player(new PlayerInputController(), Vector2.Zero);
            //Set window's title.
            Window.Title = "Yaritakunai";

            //Set the bounds to the initial value.
            updateBoundsOnChange(null, null);

            //Assigned to a bool as it is the status of if there is a settings.json.
            //If there is not, we go to the language select screen and use the default cursor colour. (etc, etc)
            bool settingsLoaded = Settings.init();
            //perfrom initialization
            Input.init();
            Camera.init();
            Player.init();
            UIS.init();
            Task.Run(()=>IME.init());
            BGMPlayer.init();
            Item.init();
            Projectile.init();

#if FORCE_RESET_SETTINGS
            settingsLoaded = false;
            Settings.defaults();
#endif
            if (settingsLoaded)
            {
                switchScreenTo(screens.title);
            }
            Settings.makeSureSettingsDirectoriesExist();
            base.Initialize();

            RenderTargetManager.init(GraphicsDevice);
            updateBoundsOnChange(null, null);
        }

        private void afterContentLoadInitialize()
        {
            WindowManager.init();
        }

        /// <summary>
        /// Overriden, as a event-driven caused stack overflow.  Odd...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            Settings.save();
            gameWM.saveWindowManager();
            playerCreationWM.saveWindowManager();
            base.OnExiting(sender, args);
        }
        /// <summary>
        /// Provides correct information about the window to the rest of the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateBoundsOnChange(object sender, EventArgs e)
        {
            zeroedWindow = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            center = new Vector2(zeroedWindow.Width / 2, zeroedWindow.Height / 2);
            //set rendertargets
            RenderTargetManager.sizeRenderTargets();
        }

        //Content declarations, except projectile and particle, which are in their own files
        //player is too I think

        //fonts
        /// <summary>
        /// Used for selected items on selectionmenus
        /// </summary>
        public static SpriteFont selectedFont;
        /// <summary>
        /// the main font for the game
        /// </summary>
        public static SpriteFont mainFont;
        /// <summary>
        /// font for displaying amount of health
        /// </summary>
        public static SpriteFont healthFont;
        /// <summary>
        /// Font for UIs.
        /// </summary>
        public static SpriteFont UIFont;
        public static SpriteFont SlantUIFont;

        //cursor
        Texture2D cursor;
        //one white pixel, can be drawn into any sort of rectangle.
        public static Texture2D notverymagicpixel;

        Effect mainShader;

        /// <summary>
        /// LoadContent loads content, and also performs initialization for after loading.
        /// </summary>
        protected override void LoadContent()
        {
            //Root directories for Images and Fonts.
            string imageRoot = "Images" + Path.DirectorySeparatorChar;
            string fontRoot = "Fonts" + Path.DirectorySeparatorChar;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load player texture(s)
            Player.spriteSheet = Content.Load<Texture2D>(imageRoot + "ヒーローイン");

            //Load particle texture.
            Particle.particle_sprites = Content.Load<Texture2D>(imageRoot + "bubble");

            //Load projectile textures.
            string projectileRoot = imageRoot + Path.DirectorySeparatorChar + "Projectiles" + Path.DirectorySeparatorChar + "Projectile";
            for (int i = 0; i < Projectile.numprojectiletypes; i++)
            {
                Projectile.projectileTextures[i] = Content.Load<Texture2D>(projectileRoot + i);
            }

            //Load cursor and drawing pixel.
            cursor = Content.Load<Texture2D>(imageRoot + "cursor");
            notverymagicpixel = Content.Load<Texture2D>(imageRoot + "pixel");

            //Load shaders.
            mainShader = Content.Load<Effect>("mainshader");

            //Load fonts.
            mainFont = Content.Load<SpriteFont>(fontRoot + "mainFont");
            selectedFont = Content.Load<SpriteFont>(fontRoot + "selectedFont");
            healthFont = Content.Load<SpriteFont>(fontRoot + "healthFont");
            UIFont = Content.Load<SpriteFont>(fontRoot + "UIFont");
            SlantUIFont = Content.Load<SpriteFont>(fontRoot + "SlantUIFont");

            //Load UI textures
            string UIImageRoot = imageRoot + "UIImages" + Path.DirectorySeparatorChar;
            UIWindow.tickDownBack = Content.Load<Texture2D>(UIImageRoot + "tickDownBack");
            UIWindow.tickDownSymbol = Content.Load<Texture2D>(UIImageRoot + "tickDownSymbol");
            UIWindow.tickDownBorder = Content.Load<Texture2D>(UIImageRoot + "tickDownSelectBorder");
            UIWindow.removeToParent = Content.Load<Texture2D>(UIImageRoot + "removeToParent");
            UIWindow.removeToParentBorder = Content.Load<Texture2D>(UIImageRoot + "removeToParentSelectBorder");
            UIWindow.close = Content.Load<Texture2D>(UIImageRoot + "close");
            UIWindow.closeBorder = Content.Load<Texture2D>(UIImageRoot + "closeSelectBorder");


            //Load NPC textures
            NPC.npcSprites[0] = Content.Load<Texture2D>(imageRoot + "クーロン");

            //load block textures
            string blockRoot = imageRoot + Path.DirectorySeparatorChar + "Blocks" + Path.DirectorySeparatorChar + "Block";
            for (int i = 0; i < Block.blockAmount; i++)
            {
                Block.blockTextures[i] = Content.Load<Texture2D>(blockRoot + i);
            }
            //load item textures
            string itemRoot = imageRoot + Path.DirectorySeparatorChar + "Items" + Path.DirectorySeparatorChar + "Item";
            for (int i = 0; i < Item.numItems; i++)
            {
                Item.itemTextures[i] = Content.Load<Texture2D>(itemRoot + i);
            }

            BGMPlayer.LoadSongs(Content);
            afterContentLoadInitialize();
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// GameTime from last update. This is not a replacement for being passed the gameTime.
        /// </summary>
        public static GameTime lastUpdateTime;

        /// <summary>
        /// Main update loop, mainly calls other update functions and pieces them together
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Always save the gametime.
            lastUpdateTime = gameTime;
            //Always want input.
            Input.update();

            if (IsActive)
            {
                switch (currentScreen)
                {
                    case screens.title:
                        //let the menu do its own clicking
                        UIS.mainMenu.update(this);

                        break;
                    case screens.mainGame:
                        UIS.anyClicked = false;
                        gameWM.update(gameTime);
                        currentPlayer.update(gameTime);

                        Camera.update(gameTime);
                        //Update all particles
                        foreach (Particle p in Particle.active)
                        {
                            p.update(gameTime);
                        }
                        Particle.purge();

                        //update items
                        foreach (ItemInWorld item in ItemInWorld.active)
                        {
                            item.update(gameTime);
                        }
                        ItemInWorld.purge();

                        //Update projectiles
                        foreach (Projectile p in Projectile.active)
                        {
                            p.update(gameTime);
                            currentPlayer.checkHitboxAndReact(p);
                            //TODO: Add collision hitbox checking (for stuff other than player, lol)
                            //NPC checking only occurs if it harms NPCs (because you have to check every. single. npc.)
                            //maybe seperate projectiles into what they can hurt into lists.
                            //it's O(N*M) N=proj, M=npc
                            if (p.harmsEnemies)
                            {
                                //check for harming NPCs
                                foreach (NPC n in NPC.Active)
                                {
                                    n.checkHitboxAndReact(p);
                                }
                            }
                        }
                        Projectile.purge();
                        //update all NPCs
                        for (int i = 0; i < NPC.Active.Count; i++)
                        {
                            //NPC.Active[i].AIupdate(gameTime, NPC.Active[i]);
                            NPC.Active[i].update(gameTime);
                        }
                        NPC.purge();


                        if (!UIS.anyClicked && cursorItem != null && Input.mouseMainButton)
                        {
                            cursorItem.use();
                        }

                        //Pause if the pause is input.
                        if (Input.pause)
                            Transitions.SetupAndBegin(GraphicsDevice, this, Transitions.Fade, screens.mainGame, screens.gamePaused);
                        break;

                    case screens.gamePaused:
                        //Unpause if pause is input.
                        if (Input.pause)
                            Transitions.SetupAndBegin(GraphicsDevice, this, Transitions.Fade, screens.gamePaused, screens.mainGame);
                        //There is also a menu. let it click itself
                        UIS.pauseMenu.update(this);

                        break;

                    case screens.langSelect:
                        UIS.langSelect.update(this);
                        break;
                    case screens.transitioning:
                        Transitions.update(gameTime);
                        break;
                    case screens.characterSelect:
                        UIS.cssubmenu.update(this);
                        UIS.characterSelect.update(this);
                        break;
                    case screens.characterCreate:
                        playerCreationWM.update(gameTime);
                        break;
                }

            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws a frame of the game to a rendertarget.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="target"></param>
        public void drawFrameToRenderTarget(screens screen, RenderTarget2D target)
        {
            //save information & set to values
            screens pastScreen = currentScreen;
            currentScreen = screen;

            bool cursorShown = isCursorDrawn;
            isCursorDrawn = false;

            bool controllerCursorShown = isPsuedoCursorDrawn;
            isPsuedoCursorDrawn = false;
            //Set to provided target and draw.
            RenderTargetManager.setRenderTarget(target);
            Draw(lastDrawTime);
            //Reset graphics device.
            RenderTargetManager.unsetRenderTarget();
            //Reset screen and cursor.
            currentScreen = pastScreen;
            isCursorDrawn = cursorShown;
            isPsuedoCursorDrawn = controllerCursorShown;
        }

        /// <summary>
        /// gametime at last draw call. This is not a replacement for being passed the gameTime.
        /// </summary>
        public static GameTime lastDrawTime;

        /// <summary>
        /// this is very necessary, it draws the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            lastDrawTime = gameTime;
            GraphicsDevice.Clear(Color.Purple);
            switch (currentScreen)
            {
                case screens.title:
                    UIS.mainMenu.draw();
                    break;
                case screens.mainGame:
                    //draw game normally
                    drawMainGame(gameTime, 0);
                    gameWM.draw(spriteBatch);
                    spriteBatch.Begin();
                    //draw the cursor item here
                    if (isCursorDrawn && cursorItem != null)
                    {
                        spriteBatch.Draw(Item.itemTextures[cursorItem.id], Input.mouseLocation + new Vector2(15, 25), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                    }
                    if (isPsuedoCursorDrawn && pseudoCursorItem != null)
                    {
                        spriteBatch.Draw(Item.itemTextures[pseudoCursorItem.id], Input.pseudoMouseLocation + new Vector2(15, 25), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                    }
                    spriteBatch.End();
                    //ui after that

                    //Debug. Is just seperate.
                    DebugRenderer.draw(spriteBatch, mainFont);
                    break;
                case screens.gamePaused:
                    //draw the game with the 'pause' darkening shader
                    drawMainGame(gameTime, 1);
                    //Debug over all.
                    DebugRenderer.draw(spriteBatch, mainFont);
                    //Pause menu last.
                    UIS.pauseMenu.draw();
                    break;
                case screens.langSelect:
                    //Draw appropriate menu.
                    UIS.langSelect.draw();
                    break;
                case screens.transitioning:
                    Transitions.current.draw(spriteBatch, gameTime);
                    break;
                case screens.characterSelect:
                    UIS.cssubmenu.draw();
                    UIS.characterSelect.draw();
                    break;
                case screens.characterCreate:
                    playerCreationWM.draw(spriteBatch);
                    break;
            }
            if (isCursorDrawn)
            {
                //Draw the cursor.
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(cursor, Input.mouseLocation, Settings.cursorColour);
                spriteBatch.End();
            }
            if (isPsuedoCursorDrawn)
            {

                //Draw the cursor.
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(cursor, Input.pseudoMouseLocation, Settings.pseudoCursorColour);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void drawMainGame(GameTime gameTime, int shaderNum)
        {

            RenderTargetManager.setRenderTarget(RenderTargetManager.framePreRender);
            //background colour: Purple!
            GraphicsDevice.Clear(Color.Purple);
            //New updated real draw calls
            //which does not rely on spaghetti
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //the blocks are in the waaaaay back.
            //if buffered draw is used, they *must* be the first thing drawn.
            world.draw(spriteBatch);
            foreach (Particle particle in Particle.active)
            {
                particle.draw(spriteBatch);
            }

            //NPCs are behind the player
            foreach (NPC npc in NPC.Active)
            {
                npc.draw(spriteBatch);
            }
            //Due to immediate, draw back to front
            currentPlayer.draw(spriteBatch);
            //UI & projectiles are in front
            foreach (Projectile p in Projectile.active)
            {
                p.draw(spriteBatch);
            }
            //item draw
            foreach (ItemInWorld item in ItemInWorld.active)
            {
                item.draw(spriteBatch);
            }

            //UI over all.

            IGUI.draw();
            spriteBatch.End();
            RenderTargetManager.unsetRenderTarget();

            //draw the rendered scene to the screen.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            //Everything is shader!
            mainShader.CurrentTechnique.Passes[shaderNum].Apply();
            //Draw everything.
            spriteBatch.Draw(RenderTargetManager.framePreRender, zeroedWindow, Color.White);
            spriteBatch.End();
        }

        //event handlers for screen switching
        public static event yaritakunaiEventHandlerO switchScreenHandler = delegate { };
        public static event yaritakunaiEventHandler titleSwitchHandler = delegate { };
        public static event yaritakunaiEventHandler characterSelectSwitchHandler = delegate { };
        public static event yaritakunaiEventHandler pauseEventHandler = delegate { };
        //switching the screens, call handler for it.
        public void switchScreenTo(screens to)
        {
            currentScreen = to;
            switch (to)
            {
                case screens.title:
                    titleSwitchHandler();
                    break;
                case screens.gamePaused:
                    pauseEventHandler();
                    break;
                case screens.characterSelect:
                    characterSelectSwitchHandler();
                    break;
                default:
                    break;
            }
            switchScreenHandler(to);
        }



        public static string toCapitalCase(string input)
        {
            return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
        }
    }

    /// <summary>
    /// Handles an event with no data.
    /// </summary>
    public delegate void yaritakunaiEventHandler();
    /// <summary>
    /// Handles an event with only event args
    /// </summary>
    /// <param name="e">Eventargs to be passed</param>
    public delegate void yaritakunaiEventHandlerE(EventArgs e);
    /// <summary>
    /// Handles an event with only an object
    /// </summary>
    /// <param name="sender">object that sent the request</param>
    public delegate void yaritakunaiEventHandlerO(object sender);
    /// <summary>
    /// Handles an event with object and eventargs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void yaritakunaiEventHandlerOE(object sender, EventArgs e);
    /// <summary>
    /// Different screens the game can be in.
    /// </summary>
    enum screens
    {
        mainGame = 0,
        title = 1,
        langSelect = 2,
        settings = 3,
        gamePaused = 4,
        transitioning = 5,
        characterSelect = 6,
        characterCreate = 7
    }
}
