using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using NoiseTest;


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
                Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2 - 128);

            List<List<List<Vector3>>> isoarray = new List<List<List<Vector3>>>();
            int width = 10;
            int depth = 10;
            int height = 5;
            OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise("The world is mine!".GetHashCode()); // ADD SEED --------------------------------------------------------
            for (int w = 0; w < width; w++)
            {
                isoarray.Add(new List<List<Vector3>>());
                for (int d = 0; d < depth; d++)
                {
                    isoarray[w].Add(new List<Vector3>());
                    for (int h = 0; h < height; h++)
                    {
                        //(c1 * x, c2 * y, c3 * z);// + c3 * z + c4)
                        int c1, c2, c3, c4;
                        c1 = 1;
                        c2 = 1;
                        c3 = 1;
                        c4 = 0;
                        double val = openSimplexNoise.Evaluate(c1 * w, c2 * d, c3 * h) + 0* c3 * h + c4;
                        Console.WriteLine("x"+ w + " y" + d + " z" + h + " val" + val);
                        if (val > 0.0)
                        {
                            isoarray[w][d].Add(new Vector3(w, d, h));
                        }
                    }
                }
            }

            int rand;
            int texWidth = 64;
            int texHeight = 64;
            Color colorA = Color.White;
            Color colorB = Color.White;
            Color colorC = Color.White;
            bool topA, topB, topC;
            bool mirroredA, mirroredB, mirroredC;
            bool borderA, borderB, borderC;
            int topOrWestOrSouthA, topOrWestOrSouthB, topOrWestOrSouthC;
            for (int x = 0; x < isoarray.Count; x++)
            {
                for (int y = 0; y < isoarray[x].Count; y++)
                {
                    for (int z = 0; z < isoarray[x][y].Count; z++)
                    {   //a = west, b= south, c = top
                        bool a = false, b = false, c = false;
                        // a0 = bottom left, a1 = left, a2 = borth left
                        // b0 = bottom right, b1 = right, b2 = both right
                        // c0 = top right, c1 = top left, c2 = both top
                        int lineNumA = 0, lineNumB = 0, lineNumC = 0;

                        // Add checking for cube neighbors in cartesian view perhaps -----------------------------------------------------------------------
                        // right now I am doing only the reverse of that. checking lack of neighbors in iso

                        // if it is the top of a column
                        if (isoarray[x][y].Count - 1 == z) {
                            bool topLeft, topRight;
                            if (x > 0)
                            {
                                topLeft = isoarray[x - 1][y].Count - 1 < z;
                            }
                            else
                            {
                                topLeft = false;
                            }

                            if (y > 0)
                            {
                                topRight = isoarray[x][y - 1].Count -1 < z;
                            }
                            else
                            {
                                topRight = false;
                            }
                            
                            // draw lines for each of the top 2 sides that lacks a neighbor
                            if (topLeft && topRight) { c = true; lineNumC = 2; }
                            else if (topLeft) { c = true; lineNumC = 1; }
                            else if (topRight) { c = true; lineNumC = 0; }
                        }

                        // ADD side and bottom line handling ---------------------------------------------------

                        // right and bottom right sides required
                        // lack of a right or bottomRight neighbor
                        bool right;//, bottomRight;
                        if (x + 1 < width && y > 0)
                        {
                            //                 no east cube same level and no south east cube same level
                            right = isoarray[x][y - 1].Count - 1 < z && isoarray[x + 1][y - 1].Count - 1 < z;
                        }
                        else if (y > 0)
                        {
                            right = isoarray[x][y - 1].Count - 1 < z;
                        }
                        else
                        {
                            right = false;
                        }

                        /*if (y > 0) // unfinished
                        {
                            bottomRight = isoarray[x][y][z-1]. - 1 >= z - 1;
                        }
                        else
                        {
                            bottomRight = false;
                        }*/

                        // draw lines for each of the top 2 sides that lacks a neighbor
                        //if (right && bottomRight) { b = true; lineNumB = 2; }
                        /*else*/ if (right) { b = true; lineNumB = 1; }
                        //else if (bottomRight) { b = true; lineNumB = 0; }

                        
                        bool left;//, bottomRight;
                        if (x > 0 && y + 1 < height)
                        {
                            //                 no north cube same level and no north west cube same level
                            left = isoarray[x - 1][y].Count - 1 < z && isoarray[x - 1][y + 1].Count - 1 < z;
                        }
                        else if (x > 0)
                        {
                            left = isoarray[x - 1][y].Count - 1 < z;
                        }
                        else
                        {
                            left = false;
                        }
                        if (left) { a = true; lineNumA = 1; }

                        // if it is not the bottom cube and there is no back left? neighbor
                        //if (x>0 && y<width - 1 && isoarray[x-1][y+1].Count < z) { b = true; }
                        // if it is not the bottom cube or first cube and there is no back right? neighbor
                        //if (x > 0 && y > 0 && isoarray[x-1][y-1].Count < z) { c = true; }


                        topA = false;
                        mirroredA = false;
                        if (a)
                        {
                            borderA = true;
                            topOrWestOrSouthA = 1;
                        }
                        else
                        {
                            borderA = false;
                            topOrWestOrSouthA = 0;
                        }

                        topB = false;
                        mirroredB = true;
                        if (b)
                        {
                            borderB = true;
                            topOrWestOrSouthB = 1;
                        }
                        else
                        {
                            borderB = false;
                            topOrWestOrSouthB = 0;
                        }

                        topC = true;
                        mirroredC = false;
                        topOrWestOrSouthC = 0;
                        if (c)
                        {
                            borderC = true;
                            
                        }
                        else
                        {
                            borderC = false;
                        }

                        rand = 0;// random.Next(0, 4);
                        switch (rand)
                        {
                            case 0:
                                colorA = Color.Brown;
                                colorB = Color.Chocolate;
                                colorC = Color.Green;
                                break;
                            case 1:
                                colorA = Color.OrangeRed;
                                colorB = Color.DarkOrange;
                                colorC = Color.Red;
                                break;
                            case 2:
                                colorA = Color.Gray;
                                colorB = Color.DarkGray;
                                colorC = Color.LightGray;
                                break;
                            case 3:
                                colorA = Color.YellowGreen;
                                colorB = Color.Yellow;
                                colorC = Color.Fuchsia;
                                break;
                            case 4:
                                colorA = Color.Tan;
                                colorB = Color.LightGoldenrodYellow;
                                colorC = Color.Beige;
                                break;
                        }
                        Cube tempCube = new Cube(origin, isoarray[x][y][z], 
                            Game1.world.textureConverter.GenTex(texWidth, texHeight, colorA, isoarray[x][y][z], topA, mirroredA, borderA, topOrWestOrSouthA, lineNumA),
                            Game1.world.textureConverter.GenTex(texWidth, texHeight, colorB, isoarray[x][y][z], topB, mirroredB, borderB, topOrWestOrSouthB, lineNumB),
                            Game1.world.textureConverter.GenTex(texWidth, texHeight, colorC, isoarray[x][y][z], topC, mirroredC, borderC, topOrWestOrSouthC, lineNumC),
                            graphics, new SpriteSheetInfo(64, 96));
                        tempCube.recalcPos();
                        cubes.Add(tempCube);
                    }
                }
            }
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
