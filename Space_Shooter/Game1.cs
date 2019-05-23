using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceShooter
{
    public enum GameState {Game, GameOver}
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game 
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random Random;
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static double delay = 2f;
        private double ShootDelayElapsed;
        private static Texture2D rockTexture;
        private List<Sprite> sprites;
        private SpriteFont font;
        public static SoundEffect pew;
        public static Song gameOver;
        public static Song damage;

        public static float Score = 0;
        public float playGameOverSound = 0;
        public static GameState state = GameState.Game;

        public Game1()
        {   
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ShootDelayElapsed = delay;
            Random = new Random();

            ScreenWidth = 1280;
            ScreenHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.HardwareModeSwitch = false;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Console.WriteLine("Loading");
            // Create a new SpriteBatch, and load texture
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var shipTexture = Content.Load<Texture2D>("SpaceShipt");
            rockTexture = Content.Load<Texture2D>("rock");
            font = Content.Load<SpriteFont>("font");
            pew = Content.Load<SoundEffect>("Pew");
            gameOver = Content.Load<Song>("GameOver");
            damage = Content.Load<Song>("Damage");


            sprites = new List<Sprite>
            {
                new Ship(shipTexture)
                {
                    Position = new Vector2(ScreenWidth / 2, ScreenHeight / 2),
                    Bullet = new Bullet(Content.Load<Texture2D>("Bob"))
                }, new Rock(rockTexture)
            };

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
            //Esc to quit program
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //If player is dead the game stops

            if (state == GameState.GameOver)
            {
                playGameOverSound++;
                if (playGameOverSound == 1)
                {
                    MediaPlayer.Play(gameOver);
                }
                return;
            }
            //Shooting mechanism
            ShootDelayElapsed -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(ShootDelayElapsed <= 0f){
                Debug.WriteLine("Spawning");
                sprites.Add(new Rock(rockTexture));
                ShootDelayElapsed = delay;
            }

            //Updates the sprites
            foreach (var sprite in sprites.ToArray())
                sprite.Update(gameTime, sprites);

            PostUpdate(); //Handles all the sprites that are supposed to be dead
        }

        private void PostUpdate()
        {
            //Removes dead sprites from list.
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].IsRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
           
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.DarkGray);
            //Activates Draw methods in sprites
            

            //Draws score text and game over text
            spriteBatch.DrawString(font, "Score: " + Score, new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(font, "Lifes: " + Ship.HealthPoints, new Vector2(100, 50), Color.Black);
            if (state == GameState.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over!", new Vector2(500, 250), Color.Black);
            }
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
