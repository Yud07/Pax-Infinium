using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace pax_infinium
{
    public class TextureConverter
    {
        public GraphicsDevice graphicsDevice;
        public TextureConverter(GraphicsDevice graphicsDevice) {
            this.graphicsDevice = graphicsDevice;
        }

        public Texture2D GenRectangle(int width, int height, Color color)
        {
            var size = width * height;
            Color[] mapcolors = new Color[size];
            for (var i = 0; i < size; i++)
            {
                mapcolors[i] = color;
            }
            var tex = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            return tex;
        }

        public Texture2D GenBorderedRectangle(int width, int height, Color color)
        {
            var size = width * height;
            Color[] mapcolors = new Color[size];
            Color darkened = Color.Multiply(color, .75f);
            darkened.A = color.A;
            int i = 0;
            for (var h = 0; h < height; h++) 
            {
                for (var w = 0; w < width; w++)
                {
                    if (h < 5 || w < 5 || h > height - 6 || w > width - 6)
                    {
                        mapcolors[i] = darkened;
                    }
                    else
                    {
                        mapcolors[i] = color;
                    }
                    i++;
                }
            }
            var tex = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            return tex;
        }

        public Texture2D ConvTest()
        {
            return Convert(GenRectangle(64, 64, Color.Blue));
        }

        public Texture2D ConvTest2()
        {
            int width = 64, height = 64;
            var size = width * height;
            Color[] mapcolors = new Color[size];
            for (var i = 0; i < size; i++)
            {
                Point p = GetPoint(i, width);
                //if (p.X < p.Y)
                //{
                    mapcolors[i] = Color.Black;
                //}
                //else
                //{
                //    mapcolors[i] = Color.Gray;
                //}
            }
            Texture2D tex = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            tex = Convert2(tex);
            return tex;
        }

        public Texture2D ConvTest3()
        {
            int width = 64, height = 64;
            var size = width * height;
            Color[] mapcolors2 = new Color[size];
            for (var i = 0; i < size; i++)
            {
                Point p = GetPoint(i, width);
                //if (p.X < p.Y)
                //{
                    //mapcolors2[i] = Color.Transparent;
                //}
                //else
                //{
                    mapcolors2[i] = Color.Orange;
                //}
            }
            Texture2D tex2 = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex2.SetData(mapcolors2);
            tex2 = Convert2(tex2, true);
            return tex2;
        }

        public Texture2D GenTex(int width, int height, Color color, Vector3 coords, bool top=true, bool mirrored=false, bool border=false, int topOrWestOrSouth=0, int lineNum=0)
        { 
            var size = width * height;
            Color[] mapcolors = new Color[size];
            int i = 0;
            int temp = 0;
            for (var w = 0; w < width; w++)
            { 
                for (var h = 0; h < height; h++)
                {
                    bool trigger = false;
                    switch (topOrWestOrSouth)
                    {
                        case 0:
                            switch (lineNum)
                            {
                                case 0:
                                    if (w < 3) { trigger = true; }
                                    break;
                                case 1:
                                    if (h < 3) { trigger = true; }
                                    break;
                                case 2:
                                    if (w < 3 || h < 3) { trigger = true; }
                                    break;
                            }
                            break;
                        case 1:
                            switch (lineNum)
                            {
                                case 0:
                                    if (w > width - 4) { trigger = true; }
                                    break;
                                case 1:
                                    if (h > height - 4) { trigger = true; }
                                    break;
                                case 2:
                                    if (w > width - 4 || h > height - 4) { trigger = true; }
                                    break;
                            }
                            break;
                    }
                    if (border && trigger)
                    {
                        /*Color tempColor = Color.Multiply(Color.White, (coords.X + coords.Y) * .02f);
                        tempColor.A = 1;*/
                        mapcolors[i] = Color.Black;// tempColor;
                        temp++;
                    }
                    else if (w < 5 || w > width - 6 || h < 5 || h > height - 6)
                    {
                        Color tempColor = color;
                        //tempColor.A -= 25; // lighter
                        //if (temp == 0)
                        //{
                            //Console.WriteLine("B4: " + tempColor);
                        //}
                        // Make the border
                        if (tempColor.R * 1.25f < 256f && tempColor.G * 1.25f < 256f && tempColor.B * 1.25f < 256f)
                        {
                            tempColor = Color.Multiply(tempColor, 1.25f);
                        }
                        else
                        {
                            tempColor = Color.Multiply(tempColor, .75f);
                        }
                        tempColor.A = color.A;
                        //`tempColor = new Color(color.R * (1 - .25f), color.G * (1 - .25f), color.B * (1 - .25f));
                        //if (temp == 0)
                        //{
                            //Console.WriteLine("After: " + tempColor);
                        //}
                        mapcolors[i] = tempColor;
                        temp++;
                    }
                    else
                    {
                        mapcolors[i] = color;
                    }
                    i++;
                }
            }
            var tex = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            if (top)
            {
                return Convert(tex);
            }
            else
            {
                if (mirrored)
                {
                    return Convert2(tex, true);
                }
                else
                {
                    return Convert2(tex);
                }
            }
        }

        public Texture2D Convert(Texture2D tex)
        {
            int isoW = tex.Width * 2;
            int isoH = tex.Height;
            //Console.WriteLine("64,0: " + Game1.world.twoDToIso(new Point(64, 0)) + " 64,64: " + Game1.world.twoDToIso(new Point(64, 64)) + " 0,64: " + Game1.world.twoDToIso(new Point(0, 64)) +
                //" 0,0: " + Game1.world.twoDToIso(new Point(0, 0)));
            int isoSize = isoW * isoH;
            int texSize = tex.Width * tex.Height;
            //Console.WriteLine("isoW:" + isoW.ToString() + " isoH:" + isoH.ToString() + "isoSize:" + isoSize.ToString() + "texSize:" + texSize.ToString());
            Color[] mapcolors = new Color[isoSize];
            Color[] pixels = new Color[texSize];
            Point cart, iso;
            tex.GetData(0, new Rectangle(0, 0, tex.Width, tex.Height), pixels, 0, texSize);
            int i = 0;
            foreach (Color color in pixels)
            {
                cart = GetPoint(i, tex.Width); // Works
                iso = Game1.world.twoDToIso(cart);
                iso.X += tex.Width;
                int isoIndex = GetArrayIndex(iso.X, iso.Y, isoW);
                //Console.WriteLine("cart:" + cart.ToString() + " iso:" + iso.ToString() + "isoIndex: " + isoIndex + " pixel length: " + pixels.Length);
                //if (isoIndex >= 0 && isoIndex < isoSize - 1)
                //{
                mapcolors[isoIndex] = color;
                    //Console.WriteLine("mapcolors: " + mapcolors[isoIndex].ToString());
                //}
                /*else
                {
                    Console.WriteLine("color: " + color + " isoIndex: " + isoIndex + " iso: " + iso + " cart: " + cart);
                }*/
                i++;
            }
            var newTex = new Texture2D(graphicsDevice, isoW, isoH, false, SurfaceFormat.Color);
            newTex.SetData(mapcolors);
            return newTex;
        }

        public Texture2D Convert2(Texture2D tex, bool mirrored=false)
        {
            int isoW = tex.Width;
            int yOffset = (int)(.75 * Math.Sqrt((tex.Height / 2.0) * (tex.Height / 2.0) + tex.Width * tex.Width));
            //int isoH = yOffset + tex.Height / 2;
            int isoH = (int)(tex.Height*1.5 + 1);
            //Console.WriteLine("isoW:" + isoW.ToString() + " isoH:" + isoH.ToString());//+ " yOffset:" + yOffset.ToString());
            //Console.WriteLine("64,0: " + Game1.world.twoDToIso(new Point(64, 0)) + " 64,64: " + Game1.world.twoDToIso(new Point(64, 64)) + " 0,64: " + Game1.world.twoDToIso(new Point(0, 64)) +
            //" 0,0: " + Game1.world.twoDToIso(new Point(0, 0)));
            int isoSize = isoW * isoH;
            int texSize = tex.Width * tex.Height;
            Color[] mapcolors = new Color[isoSize];
            Color[] pixels = new Color[texSize];
            Point cart, iso;
            tex.GetData(0, new Rectangle(0, 0, tex.Width, tex.Height), pixels, 0, texSize);
            int i = 0;
            foreach (Color color in pixels)
            {
                cart = GetPoint(i, tex.Width);
                iso = Game1.world.twoDToIso2(cart, tex.Height, tex.Width, mirrored);
                //if (mirrored && cart.X == 0)
                //{
                 //   iso.Y += yOffset;
                //}
                
                //Console.WriteLine("cart:" + cart.ToString() + " iso:" + iso.ToString());
                int isoIndex = GetArrayIndex(iso.X, iso.Y, isoW);
                //Console.WriteLine("isoIndex: " + isoIndex + " pixel length: " + pixels.Length);
                //if (isoIndex >= 0 && isoIndex < isoSize - 1)
                //{
                    mapcolors[isoIndex] = color;
                    //Console.WriteLine("mapcolors: " + mapcolors[isoIndex].ToString());
                //}
                /*else
                {
                    Console.WriteLine("color: " + color + " isoIndex: " + isoIndex + " iso: " + iso + " cart: " + cart);
                }*/
                i++;
            }
            var newTex = new Texture2D(graphicsDevice, isoW, isoH, false, SurfaceFormat.Color);
            newTex.SetData(mapcolors);
            return newTex;
        }

        protected int GetArrayIndex(int x, int y, int width)
        {
            //if (x >= width) x = width - 1;
            return (y * width) + x;
        }

        protected Point GetPoint(int index, int width)
        {
            int x = index % width;
            int y = (int) Math.Floor((double)index / (double)width);
            return new Point(x, y);
        }

        public Texture2D Rotate90(Texture2D tex)
        {
            int rotW = tex.Height;
            int rotH = tex.Width;
            //Console.WriteLine("64,0: " + Game1.world.twoDToIso(new Point(64, 0)) + " 64,64: " + Game1.world.twoDToIso(new Point(64, 64)) + " 0,64: " + Game1.world.twoDToIso(new Point(0, 64)) +
            //" 0,0: " + Game1.world.twoDToIso(new Point(0, 0)));
            int texSize = tex.Width * tex.Height;
            Color[] mapcolors = new Color[texSize];
            Color[] pixels = new Color[texSize];
            Point cart, rot;
            tex.GetData(0, new Rectangle(0, 0, tex.Width, tex.Height), pixels, 0, texSize);
            int i = 0;
            foreach (Color color in pixels)
            {
                cart = GetPoint(i, tex.Width); // Works
                rot = new Point(cart.Y, cart.X);
                int rotIndex = GetArrayIndex(rot.X, rot.Y, rotW);
                //Console.WriteLine("rotIndex: " + rotIndex + " pixel length: " + pixels.Length);
                if (rotIndex >= 0 && rotIndex < texSize - 1)
                {
                    mapcolors[rotIndex] = color;
                    //Console.WriteLine("mapcolors: " + mapcolors[rotIndex].ToString());
                }
                /*else
                {
                    Console.WriteLine("color: " + color + " rotIndex: " + rotIndex + " rot: " + rot + " cart: " + cart);
                }*/
                i++;
            }
            var newTex = new Texture2D(graphicsDevice, rotW, rotH, false, SurfaceFormat.Color);
            newTex.SetData(mapcolors);
            return newTex;
        }

        public Texture2D Rotate(Texture2D tex, double angle)
        {
            Point center = new Point(tex.Width / 2, tex.Height / 2);
            List<Point> points = new List<Point>();
            points.Add(new Point(0, 0));
            points.Add(new Point(0, tex.Height - 1));
            points.Add(new Point(tex.Width - 1, 0));
            points.Add(new Point(tex.Height - 1, tex.Width - 1));
            Point rotated;
            int highest = int.MinValue;
            int rightest = int.MinValue;
            int losouthwest = int.MaxValue;
            int leftest = int.MaxValue;
            foreach (Point p in points)
            {
                rotated = new Point(0, 0);
                rotated.X = (int)Math.Floor((p.X - center.X) * Math.Cos(angle) - (p.Y - center.Y) * Math.Sin(angle) + center.X);
                rotated.Y = (int)Math.Floor((p.X - center.X) * Math.Sin(angle) + (p.Y - center.Y) * Math.Cos(angle) + center.Y);

                if (rotated.X > rightest)
                {
                    rightest = rotated.X;
                }
                if (rotated.X < leftest)
                {
                    leftest = rotated.X;
                }
                if (rotated.Y > highest)
                {
                    highest = rotated.Y;
                }
                if (rotated.Y < losouthwest)
                {
                    losouthwest = rotated.Y;
                }

            }
            int rotW = rightest - leftest;
            int rotH = highest - losouthwest;
            //Console.WriteLine("rotW: " + rotW + " rotH: " + rotH);
            //Console.WriteLine("64,0: " + Game1.world.twoDToIso(new Point(64, 0)) + " 64,64: " + Game1.world.twoDToIso(new Point(64, 64)) + " 0,64: " + Game1.world.twoDToIso(new Point(0, 64)) +
            //" 0,0: " + Game1.world.twoDToIso(new Point(0, 0)));
            int texSize = tex.Width * tex.Height;
            Color[] mapcolors = new Color[texSize];
            Color[] pixels = new Color[texSize];
            Point cart, rot;
            tex.GetData(0, new Rectangle(0, 0, tex.Width, tex.Height), pixels, 0, texSize);
            int i = 0;
            foreach (Color color in pixels)
            {
                cart = GetPoint(i, tex.Width); // Works
                rot = new Point(0, 0);
                rot.X = (int) Math.Floor((cart.X - center.X) * Math.Cos(angle) - (cart.Y - center.Y) * Math.Sin(angle) + center.X);
                rot.Y = (int) Math.Floor((cart.X - center.X) * Math.Sin(angle) + (cart.Y - center.Y) * Math.Cos(angle) + center.Y);
                int rotIndex = GetArrayIndex(rot.X, rot.Y, rotW);
                //Console.WriteLine("rotIndex: " + rotIndex + " pixel length: " + pixels.Length);
                if (rotIndex >= 0 && rotIndex < texSize - 1)
                {
                    mapcolors[rotIndex] = color;
                    //Console.WriteLine("mapcolors: " + mapcolors[rotIndex].ToString());
                }
                /*else
                {
                    Console.WriteLine("color: " + color + " rotIndex: " + rotIndex + " rot: " + rot + " cart: " + cart);
                }*/
                i++;
            }
            var newTex = new Texture2D(graphicsDevice, rotW, rotH, false, SurfaceFormat.Color);
            newTex.SetData(mapcolors);
            return newTex;
        }
        public Texture2D GenerateStarMap(int width, int height, int numStars)
        {
            var size = width * height;
            Color[] mapcolors = new Color[size];
            for (var i = 0; i < size; i++)
            {
                mapcolors[i] = Color.Transparent;
            }
            Random rand = World.Random;
            for (var i = 0; i < numStars; i++)
            {
                var n = rand.Next(size);
                mapcolors[n] = Color.White;
            }
            var tex = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            tex.SetData(mapcolors);
            return tex;
        }

        public Texture2D highlightTex(Texture2D tex)
        {
            Texture2D tempTex = new Texture2D(graphicsDevice, tex.Width, tex.Height);
            var size = tempTex.Width * tempTex.Height;
            Color[] mapcolors = new Color[size];
            tex.GetData(mapcolors);
            for (var s = 0; s < size; s++)
            {
                Color color = mapcolors[s];
                Color tempColor = color;
                tempColor = Color.Multiply(tempColor, 1.5f);//1.25f);
                tempColor.A = color.A;
                mapcolors[s] = tempColor;
            }
            tempTex.SetData(mapcolors);
            return tempTex;
        }

        public Texture2D invertTex(Texture2D tex)
        {
            Texture2D tempTex = new Texture2D(graphicsDevice, tex.Width, tex.Height);
            var size = tempTex.Width * tempTex.Height;
            Color[] mapcolors = new Color[size];
            tex.GetData(mapcolors);
            for (var s = 0; s < size; s++)
            {
                Color color = mapcolors[s];
                if (color.A > 0)
                {
                    Color tempColor = new Color(byte.MaxValue - color.R, byte.MaxValue - color.G, byte.MaxValue - color.B, color.A);
                    mapcolors[s] = tempColor;
                }
            }
            tempTex.SetData(mapcolors);
            return tempTex;
        }
    }
}
