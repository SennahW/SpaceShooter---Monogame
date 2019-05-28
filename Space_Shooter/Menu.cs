using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter;

namespace Space_Shooter
{
    public class Menu
    {
        private float whichOptionIsSelected = 0;
        private Input input; //Using this to implement costum keybindings
        private SpriteFont font;
        private KeyboardState currentKey;

        public Menu(SpriteFont _font)
        {
            font = _font;
            input = new Input
            {
                Up = Keys.W,
                Down = Keys.S,
                Enter = Keys.Enter
            };
        }
        public void Update(GameTime gameTime)
        {
            currentKey = Keyboard.GetState();
            if (currentKey.IsKeyDown(input.Up))
            {
                whichOptionIsSelected = 0;
            }
            if (currentKey.IsKeyDown(input.Down))
            {
                whichOptionIsSelected = 1;
            }
            if (currentKey.IsKeyDown(input.Enter) && whichOptionIsSelected == 0)
            {
                Game1.state = GameState.Game;
            }
            if (currentKey.IsKeyDown(input.Enter) && whichOptionIsSelected == 1)
            {
                Game1.Exit = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Spaceshooter", new Vector2(400, 200), Color.White);
            switch (whichOptionIsSelected)
            {
                case 0:
                    spriteBatch.DrawString(font, "> Start game", new Vector2(500, 300), Color.White);
                    spriteBatch.DrawString(font, "   Quit game", new Vector2(500, 350), Color.White);
                    break;
                case 1:
                    spriteBatch.DrawString(font, "   Start game", new Vector2(500, 300), Color.White);
                    spriteBatch.DrawString(font, "> Quit game", new Vector2(500, 350), Color.White);
                    break;
            }
        }
    }
}
