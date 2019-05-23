using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace SpaceShooter.Sprites
{
    public class Rock : Sprite
    {
        private Random random;
        public Rock(Texture2D texture)
            :base(texture)
        {
            random = new Random();
            LinearVelocity = Game1.Random.Next(1, 2);
            Position = SpawnPosition();
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            rotation += 0.01f;
            Position += Direction * LinearVelocity;
            foreach(var sprite in sprites)
            {
                if (sprite == this) continue;
                if (sprite.Rectangle.Intersects(this.Rectangle) && sprite.Parent is Ship)
                {
                    this.IsRemoved = true;
                    sprite.IsRemoved = true;
                    Game1.Score++;
                    Game1.delay = Math.Pow(Game1.delay, -0.5);
                }
            }
            foreach (var sprite in sprites)
            {
                if (sprite == this) continue;
                if (sprite.Rectangle.Intersects(this.Rectangle) && sprite is Ship)
                {
                    this.IsRemoved = true;
                    Ship.HealthPoints -= 1;
                    MediaPlayer.Play(Game1.damage);
                }
            }
            if (Position.X < 0 && Position.X > Game1.ScreenWidth+1)
                IsRemoved = true;

            if(Position.Y < 0 && Position.Y > Game1.ScreenHeight+1)
                IsRemoved = true;

            if(rotation >= 6.5f)
                rotation = 0f;
        }

        
        private Vector2 SpawnPosition(){
            switch(random.Next(1,4)){
                case 1:
                    Direction = new Vector2(1,0);
                    return new Vector2(0,random.Next(100,600));
                case 2:
                    Direction = new Vector2(-1,0);
                    return new Vector2(1280,random.Next(100,600));
                case 3:
                    Direction = new Vector2(0,1);
                    return new Vector2(random.Next(100,1200),0);
                case 4:
                    Direction = new Vector2(0,-1);
                    return new Vector2(random.Next(100,1200),720);
            }
            return new Vector2(0,0);
        }
    }
}
