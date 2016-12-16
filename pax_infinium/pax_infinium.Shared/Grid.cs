using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace pax_infinium
{
    public class Grid
    {
        //private Texture2D topTex;
        //private Texture2D westTex;
        //private Texture2D southTex;
        private GraphicsDeviceManager graphics;
        public List<Cube> cubes;

        public Grid(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            cubes = new List<Cube>();
            cubes.Add(new Cube(new Vector2(Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Width /
                2, Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2), Game1.world.textureConverter.ConvTest2(),
                Game1.world.textureConverter.ConvTest3(), Game1.world.textureConverter.ConvTest(), graphics, new SpriteSheetInfo(64, 64)));

            cubes.Add(new Cube(new Vector2(300, 200), Game1.world.textureConverter.Convert2(World.textureManager["m"], false), Game1.world.textureConverter.Convert2(World.textureManager["m"], true), Game1.world.textureConverter.Convert(World.textureManager["m"]), graphics, new SpriteSheetInfo(64, 64)));            
        }

        public void Update(GameTime gameTime)
        {
            foreach (Cube cube in cubes)
            {
                cube.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cube cube in cubes)
            {
                cube.Draw(spriteBatch);
            }
        }
    }
}
