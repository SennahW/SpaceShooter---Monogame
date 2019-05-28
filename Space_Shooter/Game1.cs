using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Space_Shooter;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceShooter
{
    //Gamestates: GameOver; Game;
    public enum GameState { Game, GameOver, Menu }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static bool Exit;
        public Menu menu;

        public static Random Random;
        public static int ScreenWidth;
        public static int ScreenHeight;

        //Enemy spawn delay
        public static double RockDelay = 10f;
        //Shooting delay
        private double RockDelayElapsed;
        public static double MeteorDelay = 20f;
        private double MeteorDelayElapsed;
        //Texture and font and makes the list of sprites
        private static Texture2D rockTexture;
        public static List<Sprite> sprites;
        private SpriteFont font;
        //Sound effects
        public static Song pew;
        public static Song gameOver;
        public static Song damage;

        //Game score
        public static float Score = 0;
        //Makes sure the gameover sound only is played once
        public float playGameOverSound = 0;
        //Gamestate variable
        public static GameState state = GameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            RockDelayElapsed = RockDelay;
            MeteorDelayElapsed = MeteorDelay;
            Random = new Random();

            ScreenWidth = 1080;
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
            // Create a new SpriteBatch, and load texture
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var shipTexture = Content.Load<Texture2D>("SpaceShipt");
            rockTexture = Content.Load<Texture2D>("rock");
            font = Content.Load<SpriteFont>("font");
            pew = Content.Load<Song>("Pew");
            gameOver = Content.Load<Song>("GameOver");
            damage = Content.Load<Song>("Damage");

            menu = new Menu(font);
            //Add sprites to sprites list.
            sprites = new List<Sprite>
            {
                new Ship(shipTexture)
                {
                    Position = new Vector2(ScreenWidth / 2, ScreenHeight / 2),
                    Bullet = new Bullet(Content.Load<Texture2D>("Bob"))
                },
                new Rock(rockTexture)
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

        //Resets all values for restarting the game
        public void Restart()
        {
            RockDelay = 10f;
            Score = 0f;
            playGameOverSound = 0;
            foreach (Sprite sprite in sprites)
            {
                sprite.Restart();
            }
            Ship.HealthPoints = 4;
            Ship.velocity = Vector2.Zero;
            sprites[0].RestartSprite();
            state = GameState.Game;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Esc to quit program
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)|| Exit == true)
                Exit();
            //Restarts the game
            

            //If player is dead the game stops
            if (state == GameState.GameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    Restart();
                playGameOverSound++;
                if (playGameOverSound == 1)
                {
                    MediaPlayer.Play(gameOver);
                }
                return;
            }
            if (state == GameState.Game)
            {
                //Rock spawn 
                RockDelayElapsed -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (RockDelayElapsed <= 0f)
                {
                    Debug.WriteLine("Spawning");
                    sprites.Add(new Rock(rockTexture));
                    RockDelayElapsed = RockDelay;
                }
                //Meteor spawn
                MeteorDelayElapsed -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (MeteorDelayElapsed <= 0f)
                {
                    Debug.WriteLine("Spawning");
                    sprites.Add(new Meteor(rockTexture));
                    MeteorDelayElapsed = MeteorDelay;
                }

                //Updates the sprites
                foreach (var sprite in sprites.ToArray())
                    sprite.Update(gameTime, sprites);
            }
            if (state == GameState.Menu)
            {
                menu.Update(gameTime);
            }
            PostUpdate(); //Removes all dead sprites
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

            if (state == GameState.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over!", new Vector2(500, 250), Color.Black);
            }
            if (state == GameState.Game)
            {
                //Draws score text and game over text
                spriteBatch.DrawString(font, "Score: " + Score, new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(font, "Lifes: " + Ship.HealthPoints, new Vector2(100, 50), Color.Black);
                //Draws every sprite
                foreach (Sprite sprite in sprites)
                {
                    sprite.Draw(spriteBatch);
                }
            }
            if (state == GameState.Menu)
            {
                menu.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
