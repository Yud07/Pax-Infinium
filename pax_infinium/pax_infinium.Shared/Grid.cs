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
        Random random;

        public Grid(GraphicsDeviceManager graphics)
        {
            random = new Random();
            this.graphics = graphics;
            cubes = new List<Cube>();
            origin = new Vector2(Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Width / 2, 
                Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2);
 
            int rand;
            for (int i = 0; i < 100; i++)
            {
                rand = random.Next(0, 4);
                Vector3 coords = new Vector3(random.Next(-5, 6), random.Next(-5, 6), 0);

                switch (rand)
                {
                    case 0:
                        dropCube(new Cube(origin, coords, Game1.world.textureConverter.GenTex(64, 64, Color.Brown, false),
                            Game1.world.textureConverter.GenTex(64, 64, Color.Chocolate, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Green),
                            graphics, new SpriteSheetInfo(64, 96)));
                        break;
                    case 1:
                        dropCube(new Cube(origin, coords, Game1.world.textureConverter.GenTex(64, 64, Color.OrangeRed, false),
                            Game1.world.textureConverter.GenTex(64, 64, Color.DarkOrange, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Red),
                            graphics, new SpriteSheetInfo(64, 96)));
                        break;
                    case 2:
                        dropCube(new Cube(origin, coords, Game1.world.textureConverter.GenTex(64, 64, Color.Gray, false),
                            Game1.world.textureConverter.GenTex(64, 64, Color.DarkGray, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.LightGray),
                            graphics, new SpriteSheetInfo(64, 96)));
                        break;
                    case 3:
                        dropCube(new Cube(origin, coords, Game1.world.textureConverter.GenTex(64, 64, Color.YellowGreen, false),
                            Game1.world.textureConverter.GenTex(64, 64, Color.Yellow, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Fuchsia),
                            graphics, new SpriteSheetInfo(64, 96)));
                        break;
                    case 4:
                        dropCube(new Cube(origin, coords, Game1.world.textureConverter.GenTex(64, 64, Color.Tan, false),
                             Game1.world.textureConverter.GenTex(64, 64, Color.LightGoldenrodYellow, false, true), Game1.world.textureConverter.GenTex(64, 64, Color.Beige),
                             graphics, new SpriteSheetInfo(64, 96)));
                        break;
                }
            }

            /*cubes.Add(new Cube(Vector2.Zero, new Vector2(300, 200), Game1.world.textureConverter.Convert2(World.textureManager["m"], false), 
                Game1.world.textureConverter.Convert2(World.textureManager["m"], true), Game1.world.textureConverter.Convert(World.textureManager["m"]), 
                graphics, new SpriteSheetInfo(64, 64)));*/
        }

        public void dropCube(Cube cube)
        {
            //int before = (int) cube.gridPos.Z;
            int height = 0;
            foreach (Cube c in cubes)
            {
                if (c.gridPos.X == cube.gridPos.X && c.gridPos.Y == cube.gridPos.Y)
                {
                    height += 1;
                }
            }
            cube.gridPos.Z = height;
            //Vector2 posBefore = cube.position;
            cube.recalcPos();
            //Console.WriteLine("posBefore=" + posBefore + " pos=" + cube.position + " X=" + cube.gridPos.X + " Y=" + cube.gridPos.Y +  " B4=" + before + " After=" + cube.gridPos.Z);
            cubes.Add(cube);

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
            
            foreach (Cube cube in RenderSort(cubes))
            {
                cube.Draw(spriteBatch);
            }
        }

        public List<Cube> RenderSort(List<Cube> cubes)
        {
            List<Cube> sortedCubes = cubes.OrderBy(c => (c.gridPos.X + c.gridPos.Y + c.gridPos.Z)).ToList();
            //sortedCubes.Reverse();
            //Console.WriteLine("\n");
            //foreach (Cube cube in sortedCubes)
            //{
                //Console.WriteLine("gX=" + cube.gridPos.X + " gY=" + cube.gridPos.Y + " gZ=" + cube.gridPos.Z);
            //}
            return sortedCubes;
        }
    }
}
