using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace pax_infinium
{
    public class Level
    {
        private Random random;
        public string seed;
        public Grid grid;
        public Characters characters;
        public Background background;

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = new Random();
            grid = new Grid(graphics, seed, random);
            characters = new Characters();
            background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            characters.list.Add(new Character(Vector2.Zero, Vector3.Zero, Game1.world.textureConverter.GenRectangle(32,64, Color.Black), graphics, new SpriteSheetInfo(32,64)));
        }

        public void Update(GameTime gameTime)
        {
            grid.Update(gameTime);
            characters.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            grid.Draw(spriteBatch);
            characters.Draw(spriteBatch);
        }
    }
}
