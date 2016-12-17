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
        Vector2 origin;

        public Grid(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            cubes = new List<Cube>();
            origin = new Vector2(Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Width / 2, 
                Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2);
            
            /*cubes.Add(new Cube(new Vector2(Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Width /
                2, Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2), Game1.world.textureConverter.ConvTest2(),
                Game1.world.textureConverter.ConvTest3(), Game1.world.textureConverter.ConvTest(), graphics, new SpriteSheetInfo(64, 64)));
                */
            /*cubes.Add(new Cube(origin, new Vector2(1,1), Game1.world.textureConverter.GenTex(64, 64, Color.Brown, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.Chocolate, false, true), Game1.world.textureConverter.GenTex(64,64, Color.Green), 
                graphics, new SpriteSheetInfo(64, 96)));*/
            cubes.Add(new Cube(origin, new Vector2(1, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Brown, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.Chocolate, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Green),
                graphics, new SpriteSheetInfo(64, 96)));

            /*cubes.Add(new Cube(origin, new Vector2(2, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Brown, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.Chocolate, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Green),
                graphics, new SpriteSheetInfo(64, 96)));*/

            cubes.Add(new Cube(origin, new Vector2(3, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Brown, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.Chocolate, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Green),
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(0, 1), Game1.world.textureConverter.GenTex(64, 64, Color.OrangeRed, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.DarkOrange, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Red), 
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(0, 5), Game1.world.textureConverter.GenTex(64, 64, Color.OrangeRed, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.DarkOrange, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Red),
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(5, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Gray, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.LightGray, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.DarkGray), 
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(7, 1), Game1.world.textureConverter.GenTex(64, 64, Color.YellowGreen, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.Yellow, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Fuchsia), 
                graphics, new SpriteSheetInfo(64, 96)));


            cubes.Add(new Cube(origin, new Vector2(9, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Tan, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.LightGoldenrodYellow, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Beige), 
                graphics, new SpriteSheetInfo(64, 96)));


            cubes.Add(new Cube(origin, new Vector2(11, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Teal, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.MediumSeaGreen, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Aquamarine), 
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(-1, 1), Game1.world.textureConverter.GenTex(64, 64, Color.Navy, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.MediumBlue, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Blue), 
                graphics, new SpriteSheetInfo(64, 96)));

            cubes.Add(new Cube(origin, new Vector2(-1, -5), Game1.world.textureConverter.GenTex(64, 64, Color.Navy, false),
                Game1.world.textureConverter.GenTex(64, 64, Color.MediumBlue, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Blue),
                graphics, new SpriteSheetInfo(64, 96)));


            /*cubes.Add(new Cube(Vector2.Zero, new Vector2(300, 200), Game1.world.textureConverter.Convert2(World.textureManager["m"], false), 
                Game1.world.textureConverter.Convert2(World.textureManager["m"], true), Game1.world.textureConverter.Convert(World.textureManager["m"]), 
                graphics, new SpriteSheetInfo(64, 64)));*/
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
            List<Cube> sortedCubes = cubes.OrderBy(c => (-c.gridPos.X + c.gridPos.Y)).ToList();
            sortedCubes.Reverse();
            foreach (Cube cube in sortedCubes)
            {
                cube.Draw(spriteBatch);
            }
        }
    }
}
