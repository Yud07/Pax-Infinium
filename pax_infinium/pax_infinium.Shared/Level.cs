using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace pax_infinium
{
    public class Level
    {
        private Random random;
        public string seed;
        public Grid grid;
        public Characters characters;
        
        public Background background;
        public int perspective;

        public Level(GraphicsDeviceManager graphics, string seed)
        {
            random = World.Random;
            grid = new Grid(graphics, seed, random);
            background = new Background(World.textureManager["BG-Layer"], graphics.GraphicsDevice.Viewport);
            characters = new Characters();
            characters.list.Add(new Character(grid.origin, new Vector3(5, 5, 3), Game1.world.textureConverter.GenRectangle(64, 128, Color.Blue), graphics, new SpriteSheetInfo(64, 128)));
            characters.list.Add(new Character(grid.origin, new Vector3(3, 7, 3), Game1.world.textureConverter.GenRectangle(64, 128, Color.Red), graphics, new SpriteSheetInfo(64, 128)));
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
