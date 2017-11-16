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
        //private Texture2D southwestTex;
        //private Texture2D southeastTex;
        private GraphicsDeviceManager graphics;
        public List<Cube> cubes;
        public List<Cube> cubesA;
        public List<Cube> cubesB;
        public List<Cube> cubesC;
        public List<Cube> cubesD;
        public Vector2 origin;
        public bool[,,] binaryMatrix;
        public bool[,,] binaryMatrixB;
        public bool[,,] binaryMatrixC;
        public bool[,,] binaryMatrixD;
        public int width;
        public int depth;
        public int height;
        public int c1, c2, c3, c4;
        /*public Cube highlightedCube;
        public Sprite highlight;
        public Texture2D highlightTex;*/
        public Characters characters;
        public String seed;
        public int activeView;

        public Grid(GraphicsDeviceManager graphics, string seed, int width, int depth, int height, int c1, int c2, int c3, int c4, Random random)
        {
            this.graphics = graphics;
            cubes = new List<Cube>();
            origin = new Vector2(Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Width / 2,
                Game1.world.rooms.CurrentState.cameras.CurrentState.viewport.Height / 2 - 128);
            this.width = width;
            this.depth = depth;
            this.height = height;
            this.seed = seed;

            // cartesian space
            //width = 10; // random.Next(4, 20);
            //depth = 10; // random.Next(4, 20);
            //height = 5; // random.Next(1, 10);
            //Console.WriteLine("w " + width + " d " + depth + " h " + height);
            binaryMatrix = new bool[width, depth, height];
            OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(seed.GetHashCode());
            //(c1 * x, c2 * y, c3 * z);// + c3 * z + c4)
            //BIOMES--------------------------------------------------------------------------------------------------------
            //c1 = 1; // random.Next(1, 10);
            //c2 = 1; // random.Next(1, 10);
            //c3 = 1; // random.Next(1, 10);
            //c4 = 1; // random.Next(1, 10);
           //Console.WriteLine("c1 " + c1 + " c2 " + c2 + " c3 " + c3 + " c4 " + c4);
            Console.WriteLine("seed=" + seed + " w=" + width + " h=" + height + " d=" + depth + " c1=" + c1 + " c2=" + c2 + " c3=" + c3 + " c4=" + c4);
            for (int w = 0; w < width - 1; w++) 
            {
                for (int d = 0; d < depth - 1; d++)
                {
                    for (int h = 0; h < height - 1; h++)
                    {
                        double val = openSimplexNoise.Evaluate(c1 * w, c2 * d, c3 * h) + c3 * h + c4;
                        //Console.WriteLine("x"+ w + " y" + d + " z" + h + " val" + val);
                        bool result = false;
                        if (val > c3 * h + c4)
                        {
                            result = true;
                        }
                        binaryMatrix[w, d, h] = result;
                    }
                }
            }


            binaryMatrixB = rotateBM(binaryMatrix, true, width, depth);
            binaryMatrixC = rotateBM(binaryMatrixB, true, depth, width);
            binaryMatrixD = rotateBM(binaryMatrixC, true, width, depth);

            cubesA = createGrid(binaryMatrix);
            cubesB = createGrid(binaryMatrixB);
            cubesC = createGrid(binaryMatrixC);
            cubesD = createGrid(binaryMatrixD);

            cubes = cubesA;

            //createGrid();
        }

        public int topOfColumn(Vector3 gridPos)
        {
            int x = (int) gridPos.X;
            int y = (int) gridPos.Y;
            int maxHeight = height;
            int topZ = int.MinValue;
            for (int z = 0; z < maxHeight; z++)
            {
                if (binaryMatrix[x, y, z])
                {
                    topZ = z;
                }
            }
            return topZ;
        }

        public int topOfColumn(int x, int y, int maxHeight, bool[,,] binaryMatrix)
        {
            int topZ = int.MinValue;
            for (int z = 0; z < maxHeight; z++)
            {
                if (binaryMatrix[x,y,z])
                {
                    topZ = z;
                }
            }
            return topZ;
        }       

        public void Update(GameTime gameTime)
        {
            foreach (Cube cube in cubes)
            {
                cube.Update(gameTime);
            }
            //characters.Update(gameTime);
        }

        //bool printed = false;
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IDrawable1 obj in RenderSort(cubes, characters.list)){
                /*if (!printed)
                {
                    Console.WriteLine(obj.DrawOrder());
                }*/
                obj.Draw(spriteBatch);
            }
            /*if (highlight != null)
            {
                highlight.Draw(spriteBatch);
            }*/
            /*if (!printed)
            {
                printed = true;
            }*/
        }

        private List<IDrawable1> RenderSort(List<Cube> cubes, List <Character> characters)
        {
            List<IDrawable1> sortedObjs = new List<IDrawable1>();
            sortedObjs.InsertRange(0, cubes);
            sortedObjs.InsertRange(cubes.Count, characters);
            return sortedObjs.OrderBy(o => (o.DrawOrder())).ToList();
        }

        public void onCharacterMoved()
        {
            foreach (Cube cube in cubes)
            {
                cube.onCharacterMoved();
            }
            /*foreach (Character character in characters.list)
            {
                character.onCharacterMoved();
            }*/
        }

        public void onHighlightMoved(Cube c)
        {
            foreach (Cube cube in cubes)
            {
                cube.onHighlightMoved(c);
            }
        }

        public void createGrid()
        {
            cubes = createGrid(binaryMatrix);
        }

        public List<Cube> createGrid(bool [,,] binMatrix)
        {
            int randomColor;
            int texWidth = 64;
            int texHeight = 64;
            Color colorA = Color.White;
            Color colorB = Color.White;
            Color colorC = Color.White;
            bool topA, topB, topC;
            bool mirroredA, mirroredB, mirroredC;
            bool borderA, borderB, borderC;
            int topOrWestOrSouthA, topOrWestOrSouthB, topOrWestOrSouthC;
            randomColor = 2;//random.Next(0, 5); //0;
            //Console.WriteLine("randomColor " + randomColor);
            List<Cube> tempCubes = new List<Cube>();
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < depth - 1; y++)
                {
                    for (int z = 0; z < height - 1; z++)
                    {
                        if (binMatrix[x, y, z])
                        {
                            //a = southwest, b= southeast, c = top
                            bool a = false, b = false, c = false;
                            // a0 = bottom left, a1 = left, a2 = borth left
                            // b0 = bottom right, b1 = right, b2 = both right
                            // c0 = top right, c1 = top left, c2 = both top
                            int lineNumA = 0, lineNumB = 0, lineNumC = 0;

                            // if it is the top of a column
                            if (topOfColumn(x, y, height, binaryMatrix) == z) // May want to remove this if --------------------------------------------------------------
                            {
                                bool topLeft = false, topRight = false;
                                if (x > 0)
                                {
                                    //topLeft = isoarray[x - 1][y].Count - 1 < z;
                                    topLeft = !binaryMatrix[x - 1, y, z];

                                }

                                if (y > 0)
                                {
                                    //topRight = isoarray[x][y - 1].Count - 1 < z;
                                    topRight = !binaryMatrix[x, y - 1, z];
                                }

                                // draw lines for each of the top 2 sides that lacks a neighbor
                                if (topLeft && topRight) { c = true; lineNumC = 2; }
                                else if (topLeft) { c = true; lineNumC = 1; }
                                else if (topRight) { c = true; lineNumC = 0; }
                            }

                            // right and bottom right sides required
                            // lack of a right or bottomRight neighbor
                            bool right = false;//, bottomRight;
                            if (x + 1 < width && y > 0)
                            {
                                //                 no east cube same level and no southeast east cube same level
                                //right = isoarray[x][y - 1].Count - 1 < z && isoarray[x + 1][y - 1].Count - 1 < z;
                                right = !binaryMatrix[x, y - 1, z] && !binaryMatrix[x + 1, y - 1, z];
                            }
                            else if (y > 0)
                            {
                                //right = isoarray[x][y - 1].Count - 1 < z;
                                right = !binaryMatrix[x, y - 1, z];
                            }

                            // draw lines for each of the top 2 sides that lacks a neighbor
                            //if (right && bottomRight) { b = true; lineNumB = 2; }
                            /*else*/
                            if (right) { b = true; lineNumB = 1; }
                            //else if (bottomRight) { b = true; lineNumB = 0; }


                            bool left = false;//, bottomRight;
                            if (x > 0 && y + 1 < height)
                            {
                                //                 no north cube same level and no north southwest cube same level
                                //left = isoarray[x - 1][y].Count - 1 < z && isoarray[x - 1][y + 1].Count - 1 < z;
                                left = !binaryMatrix[x - 1, y, z] && !binaryMatrix[x - 1, y + 1, z];
                            }
                            else if (x > 0)
                            {
                                //left = isoarray[x - 1][y].Count - 1 < z;
                                left = !binaryMatrix[x - 1, y, z];
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

                            switch (randomColor)
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
                            Vector3 vect = new Vector3(x, y, z);
                            Cube tempCube = new Cube(origin, vect,
                                Game1.world.textureConverter.GenTex(texWidth, texHeight, colorA, vect, topA, mirroredA, borderA, topOrWestOrSouthA, lineNumA),
                                Game1.world.textureConverter.GenTex(texWidth, texHeight, colorB, vect, topB, mirroredB, borderB, topOrWestOrSouthB, lineNumB),
                                Game1.world.textureConverter.GenTex(texWidth, texHeight, colorC, vect, topC, mirroredC, borderC, topOrWestOrSouthC, lineNumC),
                                graphics, new SpriteSheetInfo(64, 96));
                            tempCube.recalcPos();
                            tempCubes.Add(tempCube);
                        }
                    }
                }
            }
            return tempCubes;
        }

        /*public void rotate(bool clockwise)
        {
            cubes.Clear();
            rotateBinaryMatrix(clockwise);
            createGrid();
            rotateCharacters(clockwise);
        }*/

        public void rotate(bool clockwise)
        {
            if (clockwise)
            {
                activeView++;
            }
            else
            {
                activeView--;
            }

            if (activeView > 3)
            {
                activeView = 0;
            }
            if (activeView < 0)
            {
                activeView = 3;
            }

            switch (activeView)
            {
                case 0:
                    cubes = cubesA;
                    break;
                case 1:
                    cubes = cubesB;
                    break;
                case 2:
                    cubes = cubesC;
                    break;
                case 3:
                    cubes = cubesD;
                    break;
            }
            rotateCharacters(clockwise);
        }

        public void rotateBinaryMatrix(bool clockwise)
        {
            bool [,,] newBinaryMatrix = new bool[depth, width, height]; // handles different width/depth

            double degrees;

            if (clockwise)
            {
                degrees = 90.0;
            }
            else
            {
                degrees = -90.0;
            }

            for (int w = 0; w < width - 1; w++)
            {
                for (int d = 0; d < depth - 1; d++)
                {
                    for (int h = 0; h < height - 1; h++)
                    {                        
                        Point newCoords = Game1.world.rotate(new Point(w, d), deg2Rad(degrees), new Point((int)width/2 - 1, (int)depth/2 - 1));
                        //Console.WriteLine("old: x:" + w + " y:" + d + " z: " + h + " new: x:" + newCoords.X + " y:" + newCoords.Y);
                        //Console.WriteLine("newBinaryMatrix: length: " + newBinaryMatrix.Length);
                        newBinaryMatrix[newCoords.X, newCoords.Y, h] = binaryMatrix[w, d, h];
                    }
                }
            }
            width = depth;
            depth = width;
            binaryMatrix = newBinaryMatrix;
        }

        public bool [,,] rotateBM(bool [,,] oldBinaryMatrix, bool clockwise, int wid, int dep)
        {
            bool[,,] newBinaryMatrix = new bool[depth, width, height]; // handles different width/depth

            double degrees;

            if (clockwise)
            {
                degrees = 90.0;
            }
            else
            {
                degrees = -90.0;
            }

            for (int w = 0; w < wid - 1; w++)
            {
                for (int d = 0; d < dep - 1; d++)
                {
                    for (int h = 0; h < height - 1; h++)
                    {
                        Point newCoords = Game1.world.rotate(new Point(w, d), deg2Rad(degrees), new Point((int)width / 2 - 1, (int)depth / 2 - 1));
                        //Console.WriteLine("old: x:" + w + " y:" + d + " z: " + h + " new: x:" + newCoords.X + " y:" + newCoords.Y);
                        //Console.WriteLine("newBinaryMatrix: length: " + newBinaryMatrix.Length);
                        newBinaryMatrix[newCoords.X, newCoords.Y, h] = oldBinaryMatrix[w, d, h];
                    }
                }
            }
            /*width = depth;
            depth = width;
            binaryMatrix = newBinaryMatrix;*/
            return newBinaryMatrix;
        }

        public void rotateCharacters(bool clockwise)
        {
            double degrees;

            if (clockwise)
            {
                degrees = 90.0;
            }
            else
            {
                degrees = -90.0;
            }

            foreach (Character character in characters.list)
            {
                Point newCoords = Game1.world.rotate(new Point((int)character.gridPos.X, (int)character.gridPos.Y), deg2Rad(degrees), new Point((int)width / 2 - 1, (int)depth / 2 - 1));
                character.gridPos.X = newCoords.X;
                character.gridPos.Y = newCoords.Y;
                character.recalcPos();

                if (clockwise)
                {
                    if (character.direction == "nw")
                    {
                        character.direction = "ne";
                        character.sprite.tex = character.neTex;
                    }
                    else if (character.direction == "ne")
                    {
                        character.direction = "se";
                        character.sprite.tex = character.seTex;
                    }
                    else if (character.direction == "se")
                    {
                        character.direction = "sw";
                        character.sprite.tex = character.swTex;
                    }
                    else
                    {
                        character.direction = "nw";
                        character.sprite.tex = character.nwTex;
                    }
                }
                else
                {
                    if (character.direction == "nw")
                    {
                        character.direction = "sw";
                        character.sprite.tex = character.swTex;
                    }
                    else if (character.direction == "sw")
                    {
                        character.direction = "se";
                        character.sprite.tex = character.seTex;
                    }
                    else if (character.direction == "se")
                    {
                        character.direction = "ne";
                        character.sprite.tex = character.neTex;
                    }
                    else
                    {
                        character.direction = "nw";
                        character.sprite.tex = character.nwTex;
                    }
                }
            }
            onCharacterMoved();
        }

        public double deg2Rad(double degrees)
        {
            return (Math.PI * degrees) / 180.0;
        }

        public Cube getCube(int x, int y, int z)
        {
            Vector3 temp = new Vector3(x, y, z);
            foreach (Cube cube in cubes)
            {
                if (cube.gridPos == temp)
                {
                    return cube;
                }
            }
            return null;
        }
    }
}
