using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Typocalypse.Hooks;

namespace Typocalypse
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Player player;
        private EnemyManager enemyManager;
        private List<string> wordList;
        private Texture2D background, splash;
        private Song theme;
        private bool splashOn = true;
        readonly Vector2 scoreDisplayPosition = new Vector2(20, 15);
        private SpriteFont scoreDisplay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
                           {
                               PreferredBackBufferWidth = 1024,
                               PreferredBackBufferHeight = 650
                           };
            Content.RootDirectory = "Content";
            wordList = new List<string>();
            PopulateWordList();
            HookManager.KeyPress += HookManager_KeyPress;
        }

        void HookManager_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!player.IsAlive)
            {
                return;
            }
            splashOn = false;
            var matchedEnemy = enemyManager.InputManager.InputKey(e.KeyChar);
            if (matchedEnemy != null) //we matched a word
            {
                player.Shoot(matchedEnemy);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            player = new Player(this, new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth / 2, GraphicsDevice.PresentationParameters.BackBufferHeight - 50), MathHelper.ToRadians(0f), 0.5f, "player");
            enemyManager = new EnemyManager(this, player, wordList);
            player.EnemyManager = enemyManager;
            player.Y -= player.Height;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Components.Add(player);
            Components.Add(enemyManager);
            background = Content.Load<Texture2D>("background");
            splash = Content.Load<Texture2D>("splash");
            theme = Content.Load<Song>("themesong");
            scoreDisplay = Content.Load<SpriteFont>("ScoreFont");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(theme);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            base.Draw(gameTime);
            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0,0,GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(scoreDisplay, "Score: " + player.Score, scoreDisplayPosition, Color.Red, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
            if (splashOn)
            {
                spriteBatch.Draw(splash, Vector2.Zero, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }
            spriteBatch.End();
        }

        void PopulateWordList()
        {
            string path = Path.Combine(StorageContainer.TitleLocation,"Content/dict.txt"), line;
            if (File.Exists(path))
            {
                var file = new StreamReader(path); 
                try
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        wordList.Add(line);
                    }
                }
                finally
                {
                    file.Close();
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}
