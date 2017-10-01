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
            random = World.Random;
            grid = new Grid(graphics, seed, random);
            characters = new Characters();
            background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            characters.list.Add(new Character(grid.origin, new Vector3(5,5,3), Game1.world.textureConverter.GenRectangle(64, 128, Color.Blue), graphics, new SpriteSheetInfo(64,128)));
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
