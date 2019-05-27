using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Sprites
{
    public class Bullet : Sprite
    {
        //Timer for checking lifespan of bullet;
        private float timer;
        //Loads texture
        public Bullet(Texture2D texture)
            : base(texture)
        {
        }
        

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //Updates timer
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Removes bullet if it's old
            if(timer > LifeSpan)
            {
                IsRemoved = true;
            }

            Position += Direction * LinearVelocity;
        }

    }
}
