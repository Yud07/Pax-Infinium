﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using NoiseTest;
using pax_infinium.Enum;

namespace pax_infinium
{
    public class Grid : ICloneable
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

        public int colorSet;

        public int peel;

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

            colorSet = World.Random.Next(0, 5);

            cubesA = createGrid(binaryMatrix, false);
            cubesB = createGrid(binaryMatrixB, true);
            cubesC = createGrid(binaryMatrixC, false);
            cubesD = createGrid(binaryMatrixD, true);

            cubes = cubesA;

            peel = height - 1;
            //createGrid();
        }

        public int topOfColumn(Vector3 gridPos)
        {
            int x = (int) gridPos.X;
            int y = (int) gridPos.Y;
            int maxHeight = height;
            int topZ = int.MinValue;
            bool[,,] binMat = getBinMatrix();
            for (int z = 0; z < maxHeight; z++)
            {
                if (binMat[x, y, z])
                {
                    topZ = z;
                }
            }
            return topZ;
        }

        public bool TopExposed(Vector3 gridPos)
        {
            int x = (int)gridPos.X;
            int y = (int)gridPos.Y;
            int z = (int)gridPos.Z;
            bool topExposed;
            bool[,,] binMat = getBinMatrix();
            
            if (z < height - 2)
            {
                topExposed = !binMat[x, y, z + 1] && !binMat[x, y, z + 2];
            }
            else if (z < height - 1)
            {
                topExposed = !binMat[x, y, z + 1];
            }
            else
            {
                topExposed = true;
            }
            return topExposed;
        }

        /*public int topOfColumn(int x, int y, int maxHeight, bool[,,] binaryMatrix)
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
        }*/

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

        public void onCharacterMoved(Level level)
        {
            foreach (Cube cube in cubes)
            {
                cube.onCharacterMoved(level);
            }
            foreach (Character character in characters.list)
            {
                character.onCharacterMoved(level);
            }
        }

        public void onHighlightMoved(Cube c)
        {
            foreach (Cube cube in cubes)
            {
                cube.onHighlightMoved(c);
            }
            foreach (Character character in characters.list)
            {
                character.onHighlightMoved(c);
            }
        }

        /*public void createGrid()
        {
            cubes = createGrid(binaryMatrix);
        }*/

        public List<Cube> createGrid(bool [,,] binMatrix, bool flipColors)
        {
            int texWidth = 64;
            int texHeight = 64;
            Color colorA = Color.White;
            Color colorB = Color.White;
            Color colorC = Color.White;
            bool topA, topB, topC;
            bool mirroredA, mirroredB, mirroredC;
            bool borderA, borderB, borderC;
            int topOrWestOrSouthA, topOrWestOrSouthB, topOrWestOrSouthC;
            bool topExposed;
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
                            //if (topOfColumn(x, y, height, binMatrix) == z) // May want to remove this if --------------------------------------------------------------
                            //{
                                bool topLeft = false, topRight = false;
                                if (x > 0)
                                {
                                    //topLeft = isoarray[x - 1][y].Count - 1 < z;
                                    topLeft = !binMatrix[x - 1, y, z] && !binMatrix[x - 1, y, z + 1];

                                }

                                if (y > 0)
                                {
                                    //topRight = isoarray[x][y - 1].Count - 1 < z;
                                    topRight = !binMatrix[x, y - 1, z] && !binMatrix[x, y - 1, z + 1];
                                }

                                // draw lines for each of the top 2 sides that lacks a neighbor
                                if (topLeft && topRight) { c = true; lineNumC = 2; }
                                else if (topLeft) { c = true; lineNumC = 1; }
                                else if (topRight) { c = true; lineNumC = 0; }
                            //}

                            // right and bottom right sides required
                            // lack of a right or bottomRight neighbor
                            bool right = false;//, bottomRight;
                            if (x + 1 < width && y > 0)
                            {
                                //                 no east cube same level and no southeast east cube same level
                                //right = isoarray[x][y - 1].Count - 1 < z && isoarray[x + 1][y - 1].Count - 1 < z;
                                right = !binMatrix[x, y - 1, z] && !binMatrix[x + 1, y - 1, z] && !binMatrix[x + 1, y - 1, z];
                            }
                            else if (y > 0)
                            {
                                //right = isoarray[x][y - 1].Count - 1 < z;
                                right = !binMatrix[x, y - 1, z] && !binMatrix[x + 1, y - 1, z];
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
                                left = !binMatrix[x - 1, y, z] && !binMatrix[x - 1, y + 1, z] && !binMatrix[x - 1, y + 1, z];
                            }
                            else if (x > 0)
                            {
                                //left = isoarray[x - 1][y].Count - 1 < z;
                                left = !binMatrix[x - 1, y, z] && !binMatrix[x - 1, y + 1, z];
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

                            switch (colorSet)
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
                                    colorA = Color.Brown;
                                    colorB = Color.Chocolate;
                                    colorC = Color.Green;
                                    /*colorA = Color.YellowGreen;
                                    colorB = Color.Yellow;
                                    colorC = Color.Fuchsia;*/
                                    break;
                                case 4:
                                    colorA = Color.Tan;
                                    colorB = Color.LightGoldenrodYellow;
                                    colorC = Color.Beige;
                                    break;
                            }

                            if (flipColors)
                            {
                                Color temp = colorA;
                                colorA = colorB;
                                colorB = temp;
                            }

                            if (z < height - 2)
                            {
                                topExposed = !binMatrix[x, y, z + 1] && !binMatrix[x, y, z + 2];
                            }
                            else if (z < height - 1)
                            {
                                topExposed = !binMatrix[x, y, z + 1];
                            }
                            else
                            {
                                topExposed = true;
                            }
                            if (!topExposed)
                            {
                                /*float R = (colorA.R + colorB.R) / 2;
                                float G = (colorA.G + colorB.G) / 2;
                                float B = (colorA.B + colorB.B) / 2;
                                colorC = new Color(R, G, B);*/
                                colorC = Color.Multiply(colorC, .65f);
                                colorC.A = 255;
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

        public void rotate(bool clockwise, Level level)
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

            Game1.world.level.RotateCompass();
            Game1.world.level.RotateAStarPaths(clockwise);

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
            rotateCharacters(clockwise, level);
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

        public void rotateCharacters(bool clockwise, Level level)
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

            Point rotatedMoveFrom = Game1.world.rotate(new Point((int)level.movedFrom.X, (int)level.movedFrom.Y), deg2Rad(degrees), new Point((int)width / 2 - 1, (int)depth / 2 - 1));
            level.movedFrom.X = rotatedMoveFrom.X;
            level.movedFrom.Y = rotatedMoveFrom.Y;

            foreach (Character character in characters.list)
            {
                Point newCoords = Game1.world.rotate(new Point((int)character.gridPos.X, (int)character.gridPos.Y), deg2Rad(degrees), new Point((int)width / 2 - 1, (int)depth / 2 - 1));
                character.gridPos.X = newCoords.X;
                character.gridPos.Y = newCoords.Y;
                character.recalcPos();

                if (clockwise)
                {
                    if (character.direction == EDirection.Northwest)
                    {
                        character.direction = EDirection.Northeast;
                        character.sprite.tex = character.neTex;
                    }
                    else if (character.direction == EDirection.Northeast)
                    {
                        character.direction = EDirection.Southeast;
                        character.sprite.tex = character.seTex;
                    }
                    else if (character.direction == EDirection.Southeast)
                    {
                        character.direction = EDirection.Southwest;
                        character.sprite.tex = character.swTex;
                    }
                    else
                    {
                        character.direction = EDirection.Northwest;
                        character.sprite.tex = character.nwTex;
                    }
                }
                else
                {
                    if (character.direction == EDirection.Northwest)
                    {
                        character.direction = EDirection.Southwest;
                        character.sprite.tex = character.swTex;
                    }
                    else if (character.direction == EDirection.Southwest)
                    {
                        character.direction = EDirection.Southeast;
                        character.sprite.tex = character.seTex;
                    }
                    else if (character.direction == EDirection.Southeast)
                    {
                        character.direction = EDirection.Northeast;
                        character.sprite.tex = character.neTex;
                    }
                    else
                    {
                        character.direction = EDirection.Northwest;
                        character.sprite.tex = character.nwTex;
                    }
                }
            }
            onCharacterMoved(level);
        }

        public double deg2Rad(double degrees)
        {
            return (Math.PI * degrees) / 180.0;
        }
        
        public Cube getCube(Vector3 pos)
        {
            return getCube((int)pos.X, (int)pos.Y, (int)pos.Z);
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

        public void clearTransparencies()
        {
            foreach (Cube c in cubes)
            {
                c.SetAlpha(1);
            }
            foreach (Character character in characters.list)
            {
                character.SetAlpha(1);
            }
        }

        public bool isVacant(Vector3 pos)
        {
            bool vacant = true;
            //Console.WriteLine();
            foreach (Character character in characters.list)
            {
                if (character.gridPos == pos)
                {
                    vacant = false;
                    break;
                }
            }
            return vacant;
        }

        public Character CharacterAtPos(Vector3 pos)
        {
            foreach (Character character in characters.list)
            {
                if (character.gridPos == pos)
                {
                    return character;
                }
            }
            return null;
        }

        public object Clone()
        {
            Grid clone = (Grid) this.MemberwiseClone();
            clone.characters = (Characters) characters.Clone();
            return clone;
        }

        public void placeCharacters()
        {
            List<Vector3> validPlacesOne = new List<Vector3>();
            List<Vector3> validPlacesTwo = new List<Vector3>();
            foreach (Cube cube in cubes)
            {
                if (TopExposed(cube.gridPos) && cube.gridPos.Z >= height - 3)
                {
                    if (width/2 < cube.gridPos.Y)
                    {
                        validPlacesOne.Add(cube.gridPos);
                    }
                    if (width/2 > cube.gridPos.Y)
                    {
                        validPlacesTwo.Add(cube.gridPos);
                    }
                }
            }
            //int vpoc = validPlacesOne.Count / 4;
            //int vptc = validPlacesTwo.Count / 4;
            foreach (Character character in characters.list)
            {
                Vector3 pos;
                if (character.team == 0)
                {
                    pos = validPlacesOne[World.Random.Next(validPlacesOne.Count)]; // validPlacesOne[vpoc]; 
                    validPlacesOne.Remove(pos);
                    character.gridPos = pos;
                    character.Rotate(EDirection.Northeast);
                    //vpoc++;
                }
                else
                {
                    pos = validPlacesTwo[World.Random.Next(validPlacesTwo.Count)]; // validPlacesTwo[vptc];
                    validPlacesTwo.Remove(pos);
                    character.gridPos = pos;
                    character.Rotate(EDirection.Southwest);
                    //vptc++;
                }
                character.recalcPos();
            }
        }

        public void peelCubes()
        {
            foreach (Cube cube in cubes)
            {
                if (cube.gridPos.Z > peel)
                {
                    cube.SetAlpha(0f);
                }
            }
            foreach (Character character in characters.list)
            {
                if (character.gridPos.Z > peel)
                {
                    character.SetAlpha(0f);
                }
               /* else
                {
                    character.SetAlpha(1f);
                }*/
            }
        }

        public bool[,,] getBinMatrix()
        {
            bool[,,] binMat;
            switch (activeView)
            {
                case 0:
                    binMat = binaryMatrix;
                    break;
                case 1:
                    binMat = binaryMatrixB;
                    break;
                case 2:
                    binMat = binaryMatrixC;
                    break;
                case 3:
                    binMat = binaryMatrixD;
                    break;
                default:
                    binMat = binaryMatrix;
                    break;
            }
            return binMat;
        }
    }
}
