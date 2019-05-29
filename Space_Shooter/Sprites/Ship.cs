using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Sprites
{
    public class Ship : Sprite
    {
        public Bullet Bullet;

        public static Vector2 pos;

        public static Vector2 position;

        public static float HealthPoints = 4;

        private Input input; //Using this to implement costum keybindings

        public static Vector2 velocity;

        public Ship(Texture2D texture)
            : base(texture)
        {
            input = new Input
            {
                Up = Keys.W,
                Down = Keys.S,
                Left = Keys.A,
                Right = Keys.D,
                Shoot = Keys.Space,
                Brake = Keys.LeftShift,
            };
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        { 
            //Updates rotation
            pos = Position;
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();
            if (_currentKey.IsKeyDown(input.Left))
                rotation -= MathHelper.ToRadians(RotationVelocity);
            else if (_currentKey.IsKeyDown(input.Right))
                rotation += MathHelper.ToRadians(RotationVelocity);

            Direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

            //Updates position
            if (_currentKey.IsKeyDown(input.Up))
                velocity += Direction * LinearVelocity;
            else if (_currentKey.IsKeyDown(input.Down))
                velocity -= Direction * LinearVelocity;

            //Handbrake
            if (_currentKey.IsKeyDown(input.Brake) && LinearVelocity > 0)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                LinearVelocity = 0.1f;
            }

            //Shoot mechanism
            if (_currentKey.IsKeyDown(input.Shoot) &&
               _previousKey.IsKeyUp(input.Shoot))
            {
                AddBullet(sprites);
            }

            //Color of spaceship based on HP
            if (HealthPoints == 1)
            {
                whichColor = 1;
            }
            else if (HealthPoints == 2)
            {
                whichColor = 2;
            }
            else if (HealthPoints == 3)
            {
                whichColor = 3;
            }
            if (HealthPoints <= 0)
            {
                Game1.state = GameState.GameOver;
                MediaPlayer.Play(Game1.gameOver);
            }

            Position += velocity;

            //Checks if ship is outside of screen bounds and then "teleports" it back into screen!
            PositionCheck();

        }

        //Så att man är på skärmen
        private void PositionCheck()
        {
            //X-Axis
            if (Position.X > Game1.ScreenWidth + _texture.Width)
            {
                Position.X = 0 - _texture.Width + 1;
            }
            else if (Position.X < 0 - _texture.Width)
            {
                Position.X = Game1.ScreenWidth + _texture.Width - 1;
            }

            //Y-Axis
            if (Position.Y > Game1.ScreenHeight + _texture.Height)
            {
                Position.Y = 0 - _texture.Height + 1;
            }
            else if (Position.Y < 0 - _texture.Height)
            {
                Position.Y = Game1.ScreenHeight + _texture.Height - 1;
            }
        }

        //Spawns bullet
        private void AddBullet(List<Sprite> sprites)
        {
            MediaPlayer.Play(Game1.pew);
            var bullet = Bullet.Clone() as Bullet;//Clones an instance of the bullet class
            bullet.Direction = this.Direction; //Set all parameters the same except the linearvelocity (bullets gotta go fast yo!)
            bullet.Position = this.Position;
            bullet.LinearVelocity = this.LinearVelocity * 80;
            bullet.LifeSpan = 1.5f;
            bullet.Parent = this;

            sprites.Add(bullet);//Add bullet to sprite List to be drawn and updated;
        }
    }
}
