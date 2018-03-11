using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace pax_infinium
{
    public class Cube : IDrawable1
    {
        public Sprite southwest;
        //public Sprite north;
        public Sprite top;
       // public Sprite east;
        public Sprite southeast;
        public Vector2 position;
        public Vector3 gridPos;
        public Vector2 origin;
        public Texture2D southwestTex;
        public Texture2D southeastTex;
        public Texture2D topTex;
        GraphicsDeviceManager graphics;
        SpriteSheetInfo spriteSheetInfo;
        TextItem text;
        public Sprite rectangleSprite;
        public Texture2D rectangleTex;
        public Polygon topPoly;
        public bool highLight;
        public Sprite highlight;
        public Texture2D highlightTex;
        public bool invert;
        public Sprite invertSprite;
        public Texture2D invertTex;

        public Cube(Vector2 origin, Vector3 gridPos, Texture2D southwestTex, Texture2D southeastTex, Texture2D topTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.southwestTex = southwestTex;
            this.southeastTex = southeastTex;
            this.topTex = topTex;
            this.origin = origin;
            this.gridPos = gridPos;
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * southwestTex.Width), (int)(gridPos.Y * southwestTex.Height * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * southwestTex.Height * .65F;
            this.graphics = graphics;
            this.spriteSheetInfo = spriteSheetInfo;
            this.highLight = false;
            this.invert = false;

            southwest = new Sprite(southwestTex);
            southwest.position = position;
            southwest.position.X -= (int)Math.Floor((double)southwest.tex.Width / 2);// - 1;
            southwest.position.Y += (int)Math.Floor((double)southwest.tex.Height / 2);//  - 1;
            southwest.origin = new Vector2(southwestTex.Width / 2, southwestTex.Height / 2);
            southwest.scale = 1f;
            southwest.rotation = 180;

            // enables collision by moving the rectangle to the proper space
            /*southwest.rectangle.X = (int)position.X;
            southwest.rectangle.Y = (int)position.Y;
            southwest.rectangle.Width = southwest.tex.Width;
            southwest.rectangle.Height = southwest.tex.Height;*/

            southeast = new Sprite(southeastTex);
            southeast.position = position;
            southeast.position.X += (int)Math.Floor((double)southwest.tex.Width / 2);// - 1;
            southeast.position.Y += (int)Math.Floor((double)southwest.tex.Height / 2);// - 1;
            southeast.origin = new Vector2(southeastTex.Width / 2, southeastTex.Height / 2);
            southeast.scale = 1f;
            southeast.rotation = 180;

            // enables collision by moving the rectangle to the proper space
            /*southeast.rectangle.X = (int)position.X;
            southeast.rectangle.Y = (int)position.Y;
            southeast.rectangle.Width = southeast.tex.Width;
            southeast.rectangle.Height = southeast.tex.Height;
            southeast.rectangle.*/

            top = new Sprite(topTex);
            top.position = position;
            top.origin = new Vector2(topTex.Width / 2, topTex.Height / 2);
            top.scale = 1f;

            // enables collision by moving the rectangle to the proper space
            top.rectangle.X = (int) position.X;
            top.rectangle.Y = (int) position.Y;
            top.rectangle.Width = top.tex.Width;
            top.rectangle.Height = top.tex.Height;

            topPoly = new Polygon();
            /*Vector2 topOfDiamond = new Vector2(top.position.X + top.tex.Width / 2 + top.origin.X, top.position.Y + top.origin.Y);
            Vector2 bottomOfDiamond = new Vector2(top.position.X + top.tex.Width / 2 + top.origin.X, top.position.Y + top.tex.Height + top.origin.Y);
            Vector2 leftOfDiamond = new Vector2(top.position.X + top.origin.X, top.position.Y + top.tex.Height / 2 + top.origin.Y);
            Vector2 rightOfDiamond = new Vector2(top.position.X + top.tex.Width + top.origin.X, top.position.Y + top.tex.Height / 2 + top.origin.Y);
            topPoly.Lines.Add(new PolyLine(leftOfDiamond, topOfDiamond));
            topPoly.Lines.Add(new PolyLine(topOfDiamond, rightOfDiamond));
            topPoly.Lines.Add(new PolyLine(rightOfDiamond, bottomOfDiamond));
            topPoly.Lines.Add(new PolyLine(bottomOfDiamond, leftOfDiamond));*/

            /*if (gridPos == new Vector3(8, 8, 1)) {
                Console.WriteLine("Cube Top Rect:" + top.rectangle + " Pos:" + top.position + " Origin:" + top.origin);
            }*/

            text = new TextItem(World.fontManager["InfoFont"], "X: " + gridPos.X + " Y: " + gridPos.Y + " Z: " + gridPos.Z);
            text.position = position;

            /*if (gridPos == new Vector3(8, 8, 1))
            {
                // add random transparent colors
                rectangleTex = Game1.world.textureConverter.GenRectangle(top.rectangle.Width, top.rectangle.Height, Color.Red); // top.rectangle.Width, top.rectangle.Height, Color.Black);
                rectangleSprite = new Sprite(rectangleTex, graphics, spriteSheetInfo);
                rectangleSprite.position = new Vector2(top.position.X, top.position.Y);
                rectangleSprite.origin = top.origin;
                Console.WriteLine("pos:" + rectangleSprite.position);
            }*/

            /*highlightTex = Game1.world.textureConverter.highlightTex(topTex);
            highlight = new Sprite(highlightTex);
            highlight.origin = top.origin;
            highlight.position = top.position;*/
        }

        public Cube(Vector2 position, Texture2D southwestTex, Texture2D southeastTex, Texture2D topTex) // Only used for peelStatus
        {
            this.southwestTex = southwestTex;
            this.southeastTex = southeastTex;
            this.topTex = topTex;
            this.position = position;

            float scale = .25f;

            southwest = new Sprite(southwestTex);
            southwest.position = position;
            southwest.position.X -= (int)Math.Floor((double)southwest.tex.Width * scale / 2);// - 1;
            southwest.position.Y += (int)Math.Floor((double)southwest.tex.Height * scale / 2);//  - 1;
            southwest.scale = scale;
            southwest.rotation = 180;

            southeast = new Sprite(southeastTex);
            southeast.position = position;
            southeast.position.X += (int)Math.Floor((double)southwest.tex.Width * scale / 2);// - 1;
            southeast.position.Y += (int)Math.Floor((double)southwest.tex.Height * scale / 2);// - 1;
            southeast.scale = scale;
            southeast.rotation = 180;

            top = new Sprite(topTex);
            top.position = position;
            top.scale = scale;
        }

        public void recalcPos()
        {
            int width = 11;// Game1.world.level.grid.width + 1;
            int depth = 11;// Game1.world.level.grid.depth + 1;
            int height = 6;// Game1.world.level.grid.height + 1;
            float scale = (100f - width/2 - depth / 2 - height / 2 + gridPos.Z / 2 + gridPos.Y / 2 + gridPos.X / 2) / (100f - width / 2 - depth / 2 - height / 2);
            top.scale = scale;
            southwest.scale = scale;
            southeast.scale = scale;
            text.scale = scale;

            /*Vector2 newPos = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * southwestTex.Width *scale), (int)(gridPos.Y * southwestTex.Height * scale * .65f))).ToVector2();
            newPos.Y -= gridPos.Z * southwestTex.Height * scale * .65F;
            this.position = new Vector2((float)Math.Floor((double)newPos.X), (float)Math.Floor((double)newPos.Y));*/

            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * southwestTex.Width * scale), (int)(gridPos.Y * southwestTex.Height * scale * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * southwestTex.Height * scale * .65F;


            //this.position.X = position.X * scale;

            southwest.position = position;
            southwest.position.X -= (int)Math.Floor((double)southwest.tex.Width * scale / 2);// - 1;
            southwest.position.Y += (int)Math.Floor((double)southwest.tex.Height * scale / 2);

            southeast.position = position;
            southeast.position.X += (int)Math.Floor((double)southwest.tex.Width * scale / 2);// - 1;
            southeast.position.Y += (int)Math.Floor((double)southwest.tex.Height * scale / 2);// - 1;

            top.position = position;
            top.rectangle.X = (int)position.X;
            top.rectangle.Y = (int)position.Y;

            Vector2 topOfDiamond = new Vector2(top.position.X + top.tex.Width / 2 - top.origin.X, top.position.Y - top.origin.Y);
            Vector2 bottomOfDiamond = new Vector2(top.position.X + top.tex.Width / 2 - top.origin.X, top.position.Y + top.tex.Height - top.origin.Y);
            Vector2 leftOfDiamond = new Vector2(top.position.X - top.origin.X, top.position.Y + top.tex.Height / 2 - top.origin.Y);
            Vector2 rightOfDiamond = new Vector2(top.position.X + top.tex.Width - top.origin.X, top.position.Y + top.tex.Height / 2 - top.origin.Y);
            topPoly.Lines.Clear();
            topPoly.Lines.Add(new PolyLine(leftOfDiamond, topOfDiamond));
            topPoly.Lines.Add(new PolyLine(topOfDiamond, rightOfDiamond));
            topPoly.Lines.Add(new PolyLine(rightOfDiamond, bottomOfDiamond));
            topPoly.Lines.Add(new PolyLine(bottomOfDiamond, leftOfDiamond));

            if (gridPos == new Vector3(8, 8, 1))
            {
                // add random transparent colors
                /*rectangleTex = Game1.world.textureConverter.GenRectangle(top.rectangle.Width, top.rectangle.Height, Color.Red); // top.rectangle.Width, top.rectangle.Height, Color.Black);
                rectangleSprite = new Sprite(rectangleTex, graphics, spriteSheetInfo);
                rectangleSprite.position = new Vector2(top.position.X, top.position.Y);
                rectangleSprite.origin = top.origin;
                Console.WriteLine("pos:" + rectangleSprite.position);*/
            }

            darken();
            text.Text = "     (" + gridPos.X + ", " + gridPos.Y + ", " + gridPos.Z + ")";
            //"X: " + gridPos.X + " Y: " + gridPos.Y + " Z: " + gridPos.Z; //"[" + position.X + "," + position.Y + "]";
            text.position = position;

            highlightTex = Game1.world.textureConverter.highlightTex(topTex);
            highlight = new Sprite(highlightTex);
            highlight.origin = top.origin;
            highlight.position = top.position;
            highlight.scale = scale;

            invertTex = Game1.world.textureConverter.invertTex(topTex);
            invertSprite = new Sprite(invertTex);
            invertSprite.origin = top.origin;
            invertSprite.position = top.position;
            invertSprite.scale = scale;
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            southwest.Draw(spriteBatch);
            southeast.Draw(spriteBatch);
            if (invert)
            {
                invertSprite.Draw(spriteBatch);
            }
            else if (highLight)
            {
                highlight.Draw(spriteBatch);
            }
            else
            {
                top.Draw(spriteBatch);
            }
            //text.Draw(spriteBatch);
            if (rectangleTex != null)
            {
                rectangleSprite.Draw(spriteBatch);

            }
        }

        public void darken()
        {
            southwestTex = darkenTex(southwestTex);
            southeastTex = darkenTex(southeastTex);
            topTex = darkenTex(topTex);
        }

        public Texture2D darkenTex(Texture2D tex)
        {
            var size = tex.Width * tex.Height;
            Color[] mapcolors = new Color[size];
            tex.GetData(mapcolors);
            //Color[] newmapcolors = new Color[size];
            for (var s = 0; s < size; s++)
            {
                Color color = mapcolors[s];
                Color tempColor = color;
                int width, depth, height;
                width = 11;// Game1.world.level.grid.width + 1;
                depth = 11;// Game1.world.level.grid.depth + 1;
                height = 6;// Game1.world.level.grid.height + 1;
                for (int h = 0; h < width + depth + height - gridPos.Z - gridPos.Y - gridPos.X; h++) // these need to be tied to level size
                {
                    tempColor = Color.Multiply(tempColor, .97f);
                }
                tempColor.A = color.A;
                mapcolors[s] = tempColor;
            }
            tex.SetData(mapcolors);
            return tex;
        }

        public Texture2D border(Texture2D tex, int topOrWestOrSouth)
        {
            var size = tex.Width * tex.Height;
            Color[] mapcolors = new Color[size];
            tex.GetData(mapcolors);

            List<int> maxes = new List<int>();
            List<int> mins = new List<int>();

            int index = 0;
            int max;
            int min;
            for (int w = 0; w < tex.Width; w++)
            {
                max = 0;
                min = int.MaxValue;
                for (int h = 0; h < tex.Height; h++)
                {
                    if (mapcolors[index].A != 0)
                    {
                        max = h;
                        if (h < min)
                        {
                            min = h;
                        }
                    }
                    index++;
                }
                maxes.Add(max);
                mins.Add(min);

            }
            //Console.WriteLine();
            //foreach (int m in maxes)
            //{
                //Console.Write(m + " ");
            //}

            index = 0;
            for (int w = 0; w < tex.Width; w++)
            {
                for (int h = 0; h < tex.Height; h++)
                {
                    bool trigger = false;
                    switch (topOrWestOrSouth)
                    {
                        case 0:
                            // top left or top right
                            if (((h == mins[w] || h == mins[w] + 1 && mins[w] != 0) || (h == maxes[w]  || h == maxes[w] - 1 && maxes[w] != 63)) &&
                                w < topTex.Width / 2){ trigger = true; }
                                break;
                        case 1:
                            if (((h == mins[w] || h == mins[w] + 1 && mins[w] != 0) || (h == maxes[w] || h == maxes[w] - 1 && maxes[w] != 63)) &&
                                w < southwestTex.Width / 2) { trigger = true; }
                            break;
                        case 2:
                            Point rotated = new Point(0, 0);
                            rotated.X = (int)Math.Floor((w) * Math.Cos(180) - (h) * Math.Sin(180));
                            rotated.Y = (int)Math.Floor((w) * Math.Sin(180) + (h) * Math.Cos(180));
                            if (rotated.X == 0 || rotated.Y == 0) { trigger = true; }
                            break;
                    }

                    if (trigger)
                    {
                        mapcolors[index] = Color.White;
                    }

                    index++;
                }          
                
            }
            tex.SetData(mapcolors);
            return tex;
        }

        /*
        public Cube copy()
        {
            return new Cube(position, southwestTex, southeastTex, topTex, graphics, spriteSheetInfo);
        }
        */

        public Vector3 getGridPos()
        {
            return gridPos;
        }

        public int DrawOrder()
        {
            return (int) (gridPos.X + gridPos.Y + gridPos.Z);
        }

        public void SetAlpha(float alpha)
        {
            top.alpha = alpha;
            southwest.alpha = alpha;
            southeast.alpha = alpha;
            text.alpha = alpha;
            highlight.alpha = alpha;
            invertSprite.alpha = alpha;
        }

        public void onCharacterMoved(Level level)
        {
            //SetAlpha(1f);
            Character character = level.grid.characters.list[0];
            //foreach (Character character in level.grid.characters.list)
            //{
                //Cube tempCube = Game1.world.level.grid.getCube(character.gridPos);
                if (DrawOrder() > character.DrawOrder() && Vector2.Distance(position, character.position) < 100 && gridPos.Z > character.gridPos.Z)
                {
                    SetAlpha(.5f);
                }
            //}
        }

        public void onHighlightMoved(Cube c)
        {
            SetAlpha(1f);
            Character characterOnCube = Game1.world.level.grid.CharacterAtPos(c.gridPos);
            if (characterOnCube != null)
            {
                if (DrawOrder() > characterOnCube.DrawOrder() && Vector2.Distance(position, characterOnCube.position) < 100 && gridPos.Z > characterOnCube.gridPos.Z)
                {
                    SetAlpha(.5f);
                }
            }
            else
            {
                if (DrawOrder() > c.DrawOrder() && Vector2.Distance(position, c.position) < 100 && gridPos.Z > c.gridPos.Z)
                {
                    SetAlpha(.5f);
                }
            }
        }

        public bool isAdjacent(Vector3 v, int zTolerance = 1)
        {
            return Game1.world.linearCubeDist(gridPos, v) == 1 && Math.Abs(gridPos.Z - v.Z) <= zTolerance;
        }

        public bool clearPath(Vector3 v, Grid g)
        {
            Vector3 lower;
            Vector3 higher;

            if (v.Z <= gridPos.Z)
            {
                lower = v;
                higher = gridPos;
            }
            else
            {
                higher = v;
                lower = gridPos;
            }

            bool result = true;
            for (int z = ((int)lower.Z) + 1; z < g.height && z < higher.Z + 2; z++)
            {
                Cube c = g.getCube((int)lower.X, (int)lower.Y, (int)z);
                if (c != null)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool canMoveAdjacentTo(Vector3 v, Grid g, int zTolerance = 1)
        {
            return isAdjacent(v, zTolerance) && clearPath(v, g);
        }
    }
}
