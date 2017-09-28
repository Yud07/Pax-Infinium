using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace pax_infinium
{
    public class Cube
    {
        public Sprite west;
        public Sprite north;
        public Sprite top;
        public Sprite east;
        public Sprite south;
        public Vector2 position;
        public Vector3 gridPos;
        public Vector2 origin;
        public Texture2D westTex;
        public Texture2D southTex;
        public Texture2D topTex;
        GraphicsDeviceManager graphics;
        SpriteSheetInfo spriteSheetInfo;
        TextItem text;

        public Cube(Vector2 origin, Vector3 gridPos, Texture2D westTex, Texture2D southTex, Texture2D topTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.westTex = westTex;
            this.southTex = southTex;
            this.topTex = topTex;
            this.origin = origin;
            this.gridPos = gridPos;
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * westTex.Width), (int)(gridPos.Y * westTex.Height * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * westTex.Height * .65F;
            this.graphics = graphics;
            this.spriteSheetInfo = spriteSheetInfo;

            west = new Sprite(westTex, graphics, spriteSheetInfo);
            west.position = position;
            west.position.X -= west.tex.Width / 2;// - 1;
            west.position.Y += west.tex.Height / 2;//  - 1;
            west.origin = new Vector2(westTex.Width / 2, westTex.Height / 2);
            west.scale = 1f;
            west.rotation = 180;
            //west.rotation = 25;

            south = new Sprite(southTex, graphics, spriteSheetInfo);
            south.position = position;
            south.position.X += west.tex.Width / 2;// - 1;
            south.position.Y += west.tex.Height / 2;// - 1;
            south.origin = new Vector2(southTex.Width / 2, southTex.Height / 2);
            south.scale = 1f;
            south.rotation = 180;
            //south.rotation = (float)(63.5 / 2.0);

            top = new Sprite(topTex, graphics, spriteSheetInfo);
            top.position = position;
            top.origin = new Vector2(topTex.Width / 2, topTex.Height / 2);
            top.scale = 1f;
            top.rectangle.X = (int) position.X;
            top.rectangle.Y = (int) position.Y;
            if (gridPos == new Vector3(8, 8, 1)) {
                Console.WriteLine("cubeRect:" + top.rectangle);
            }

            text = new TextItem(World.fontManager["InfoFont"], "X: " + gridPos.X + " Y: " + gridPos.Y + " Z: " + gridPos.Z);
            text.position = position;
        }

        public void recalcPos()
        {
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * westTex.Width), (int)(gridPos.Y * westTex.Height * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * westTex.Height * .65F;

            west.position = position;
            west.position.X -= west.tex.Width / 2;// - 1;
            west.position.Y += west.tex.Height / 2;

            south.position = position;
            south.position.X += west.tex.Width / 2;// - 1;
            south.position.Y += west.tex.Height / 2;// - 1;

            top.position = position;

            darken();
            text.Text = "X: " + gridPos.X + " Y: " + gridPos.Y + " Z: " + gridPos.Z; //"[" + position.X + "," + position.Y + "]";
            text.position = position;
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            west.Draw(spriteBatch);
            south.Draw(spriteBatch);
            top.Draw(spriteBatch);
            text.Draw(spriteBatch);

        }

        public void darken()
        {
            westTex = darkenTex(westTex);
            southTex = darkenTex(southTex);
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
                for (int h = 0; h < 6 - gridPos.Z + 11 - gridPos.Y + 11 - gridPos.X; h++)
                {
                    tempColor = Color.Multiply(tempColor, .96f);
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
                                w < westTex.Width / 2) { trigger = true; }
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
            return new Cube(position, westTex, southTex, topTex, graphics, spriteSheetInfo);
        }
        */
    }
}
