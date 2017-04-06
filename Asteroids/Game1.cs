using System.Collections.Generic;
using System;

// MonoGame calls.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Asteroids calls.
using Asteroids.Entities;
using Asteroids.Tools;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 MainGame;
        public static Vector2 ScreenCenter = new Vector2(0.5f, 0.5f);
        public static Vector2 ScreenBounds = new Vector2(1.0f, 1.0f);
        public static Vector2 DisplayBounds = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        public static GraphicsDeviceManager GraphicsDM;
        public static ShapeDrawer Pen;
        public static SpriteFont Small;

        public static Random Random;
        
        public static void UpdateScreenDimensions(float width, float height)
        {
            MainGame.UpdateScreen(width, height);
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ShapeDrawer shapeDrawer;
               
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GraphicsDM = graphics;
            Random = new Random();
            MainGame = this;
        }
        
    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            UpdateScreen(800, 600);
            StateManager.Initialize();
            InputManager.Initialize();    

            base.Initialize();
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            shapeDrawer = new ShapeDrawer(spriteBatch, Content.Load<SpriteFont>("assets/fonts/mainfont"), GraphicsDevice);

            Pen = shapeDrawer;
            Small = Content.Load<SpriteFont>("assets/fonts/smallfont");

            // Get the textures.
            Dictionary<StateManager.TextureIDs, Texture2D> textures = new Dictionary<StateManager.TextureIDs, Texture2D>();            
            textures.Add(StateManager.TextureIDs.Button , Content.Load<Texture2D>("assets/ui/button_base"));
            textures.Add(StateManager.TextureIDs.Asteroid1, Content.Load<Texture2D>("assets/asteroids/asteroid_01"));
            textures.Add(StateManager.TextureIDs.Asteroid2, Content.Load<Texture2D>("assets/asteroids/asteroid_02"));
            textures.Add(StateManager.TextureIDs.Asteroid3, Content.Load<Texture2D>("assets/asteroids/asteroid_03"));
            textures.Add(StateManager.TextureIDs.Player, Content.Load<Texture2D>("assets/player/spaceship"));
            textures.Add(StateManager.TextureIDs.Bullet, Content.Load<Texture2D>("assets/player/bullet"));
            textures.Add(StateManager.TextureIDs.Test, Content.Load<Texture2D>("assets/player/bullet"));

            // Create the states.
            StateManager.CreateStates(spriteBatch, shapeDrawer, textures);            
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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           if (StateManager.GetState() == StateManager.States.Quit)
                Exit();

            // TODO: Add your update logic here
            UpdateBack(gameTime);
            UpdateGUI(gameTime);

            base.Update(gameTime);
        }

        private void UpdateBack(GameTime gameTime)
        {
            // TODO.
            InputManager.Update(gameTime);
            StateManager.Update(gameTime);
        }

        private void UpdateGUI(GameTime gameTime)
        {
            StateManager.UpdateGUI(gameTime);            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(StateManager.Background);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            Draw();
            DrawGUI();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Draw()
        {
            StateManager.Draw();
        }

        private void DrawGUI()
        {
            StateManager.DrawGUI(shapeDrawer);
        }

        private void DrawTestGUI()
        {
            int height = (int)shapeDrawer.StringDimensions("A").Y;

            shapeDrawer.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y - height, Color.White, "This is a left test.", ShapeDrawer.LEFT_ALIGN);

            shapeDrawer.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y, Color.White, "This is a center test.", ShapeDrawer.CENTER_ALIGN);

            shapeDrawer.DrawString((int)ScreenCenter.X, (int)ScreenCenter.Y + height, Color.White, "This is a right test.", ShapeDrawer.RIGHT_ALIGN);

            shapeDrawer.DrawString(0, 0, Color.White, "This is a default test.");
        }

        // Update dimension variables.
        public void UpdateScreen(float screenWidth, float screenHeight)
        {
            graphics.PreferredBackBufferWidth = (int)Math.Abs(screenWidth);
            graphics.PreferredBackBufferHeight = (int)Math.Abs(screenHeight);
            graphics.ApplyChanges();

            ScreenBounds = new Vector2(screenWidth, screenHeight);
            ScreenCenter = ScreenBounds * 0.5f;
            DisplayBounds = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            // Reset window position.
            int w = graphics.PreferredBackBufferWidth;
            int h = graphics.PreferredBackBufferHeight;

            int dW = (int)DisplayBounds.X;
            int dH = (int)DisplayBounds.Y;

            if (w != dW)
            {
                graphics.IsFullScreen = false;
                this.Window.Position = new Point(((int)(dW / 2) - ((int)ScreenCenter.X)), ((int)(dH / 2) - ((int)ScreenCenter.Y)));
            }
            else
            {
                graphics.IsFullScreen = true;
            }

            graphics.ApplyChanges();
        }

        // Update dimension variables.
        public void UpdateScreen()
        {
            UpdateScreen(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

    }
}
