using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Sprites
{
    public class Sprite : ICloneable
    {
        protected Texture2D _texture;
        public float whichColor = 0;
        public float colorCycle = 0;
        public float rotation;
        public KeyboardState _currentKey;
        public KeyboardState _previousKey;

        public Vector2 Position;
        public Vector2 Origin;

        public Vector2 Direction;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 0.1f;

        public Sprite Parent;

        public float LifeSpan = 0.5f;

        
        public bool IsRemoved = false;

        public void Restart()
        {
            whichColor = 0;                                
            colorCycle = 0;
            rotation = 0;
            Position = Vector2.Zero;
            Direction = Vector2.Zero;
            RotationVelocity = 3f;
            LinearVelocity = 0.1f;
            LifeSpan = 0f;
        }
        
        public Rectangle Rectangle //For basic collision detection
        {
            get 
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }
        
        public void RestartSprite()
        {
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (whichColor == 0)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, rotation, Origin, 1, SpriteEffects.None, 0f);
            }
            else if (whichColor == 1)
            {
                spriteBatch.Draw(_texture, Position, null, Color.Red, rotation, Origin, 1, SpriteEffects.None, 0f);
            }
            else if (whichColor == 2)
            {
                spriteBatch.Draw(_texture, Position, null, Color.DarkOrange, rotation, Origin, 1, SpriteEffects.None, 0f);
            }
            else if (whichColor == 3)
            {
                spriteBatch.Draw(_texture, Position, null, Color.Orange, rotation, Origin, 1, SpriteEffects.None, 0f);
            }
            else if (whichColor == 4)
            {
                spriteBatch.Draw(_texture, Position, null, Color.LightSalmon, rotation, Origin, 1, SpriteEffects.None, 0f);
            }
        }
           

        public object Clone()
        {
            return this.MemberwiseClone();
        }


    }
}
