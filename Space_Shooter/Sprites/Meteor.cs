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
    public class Meteor : Sprite
    {
        public Meteor(Texture2D texture)
            :base(texture)
        {
            LinearVelocity = Game1.Random.Next(1, 2);
            Position = SpawnPosition();
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //WhichColor for drawing color
            whichColor = 5;

            //Targeting
            Direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            Position += Direction * LinearVelocity / 2;
            Vector2 diff = Position - Ship.pos;
            rotation = (float)Math.Atan2(diff.Y, diff.X) - (float)(Math.PI);

            //Checks hitboxex
            foreach (var sprite in sprites)
            {
                if (sprite == this) continue;
                if (sprite.Rectangle.Intersects(this.Rectangle) && sprite.Parent is Ship)
                {
                    this.IsRemoved = true;
                    sprite.IsRemoved = true;
                    Game1.Score++;
                    Game1.MeteorDelay = Math.Pow(Game1.MeteorDelay, -0.1);
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

            //Removes if off screen
            if (Position.X < 0 && Position.X > Game1.ScreenWidth+1)
                IsRemoved = true;

            if(Position.Y < 0 && Position.Y > Game1.ScreenHeight+1)
                IsRemoved = true;

            if(rotation >= 6.5f)
                rotation = 0f;
        }

        //Select random spwanposition
        private Vector2 SpawnPosition(){
            switch(Game1.Random.Next(1,4)){
                case 1:
                    Direction = new Vector2(1,0);
                    return new Vector2(0,Game1.Random.Next(100,600));
                case 2:
                    Direction = new Vector2(-1,0);
                    return new Vector2(1280,Game1.Random.Next(100,600));
                case 3:
                    Direction = new Vector2(0,1);
                    return new Vector2(Game1.Random.Next(100,1200),0);
                case 4:
                    Direction = new Vector2(0,-1);
                    return new Vector2(Game1.Random.Next(100,1200),720);
            }
            return new Vector2(0,0);
        }
    }
}
