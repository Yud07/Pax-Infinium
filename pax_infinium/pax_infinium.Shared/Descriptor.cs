using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace pax_infinium
{
    public class Descriptor
    {
        public Polygon poly;
        String text;
        Vector2 pos;
        Sprite backing;
        List<TextItem> textItems;
        int width;
        int height;
        public bool trigger;

        public Descriptor(Polygon poly, String text)
        {
            this.poly = poly;
            int leftMost = int.MaxValue;
            int rightMost = int.MinValue;
            int topMost = int.MaxValue;
            int bottomMost = int.MinValue;
            foreach (PolyLine l in poly.Lines)
            {
                leftMost = (int) Math.Min(l.Start.X, leftMost);
                rightMost = (int) Math.Max(l.Start.X, rightMost);
                topMost = (int) Math.Min(l.Start.Y, topMost);
                bottomMost = (int) Math.Max(l.Start.Y, bottomMost);

                leftMost = (int)Math.Min(l.End.X, leftMost);
                rightMost = (int)Math.Max(l.End.X, rightMost);
                topMost = (int)Math.Min(l.End.Y, topMost);
                bottomMost = (int)Math.Max(l.End.Y, bottomMost);
            }

            textItems = new List<TextItem>();

            textItems.Add(new TextItem(World.fontManager["Trajanus Roman 24"], text));
            TextItem textItem = textItems.ToArray()[0];
            textItem.origin = Vector2.Zero;
            textItem.color = Color.White;
            width = textItem.rectangle.Width + 20;
            height = textItem.rectangle.Height * 4;           

            Vector2 topLeft = new Vector2(leftMost, topMost - 5 - height);
            Vector2 bottomLeft = new Vector2(leftMost, bottomMost + 5);
            Vector2 topRight = new Vector2(rightMost - width, topMost - 5 - height);
            Vector2 bottomRight = new Vector2(rightMost - width, bottomMost + 5);

            if (!(textInBounds(topLeft) || textInBounds(bottomLeft) || textInBounds(topRight) || textInBounds(bottomRight)))
            {
                int lines = 1;
                while (true)
                {
                    lines++;
                    String[] splitText = text.Split();
                    List<String> stringLines = new List<String>();
                    int i = 0;
                    int l = 1;
                    String temp = "";
                    int tempChars = 0;
                    foreach (String s in splitText)
                    {
                        temp += s + " ";
                        tempChars += s.Length;
                        if (tempChars >= (text.Length / lines) * l || splitText.Length - 1 == i)
                        {
                            stringLines.Add(temp);
                            temp = "";
                            l++;
                        }
                        i++;
                    }

                    textItems.Clear();
                    width = 0;
                    height = 0;
                    int k = 0;
                    foreach (String s in stringLines)
                    {
                        textItems.Add(new TextItem(World.fontManager["Trajanus Roman 24"], s));
                        TextItem tempTextItem = textItems.ToArray()[k];
                        tempTextItem.origin = Vector2.Zero;
                        tempTextItem.color = Color.White;
                        width = Math.Max(width, tempTextItem.rectangle.Width + 20);
                        height += textItem.rectangle.Height * 4;
                        k++;
                    }
                    topLeft = new Vector2(leftMost, topMost - 5 - height);
                    bottomLeft = new Vector2(leftMost, bottomMost + 5);
                    topRight = new Vector2(rightMost - width, topMost - 5 - height);
                    bottomRight = new Vector2(rightMost - width, bottomMost + 5);
                    if (textInBounds(topLeft) || textInBounds(bottomLeft) || textInBounds(topRight) || textInBounds(bottomRight) || lines > 10)
                    {
                        break;
                    }

                }
            }

            if (textInBounds(topLeft))
            {
                Console.WriteLine("Chose topLeft");
                pos = topLeft;
            }
            else if (textInBounds(bottomLeft))
            {
                Console.WriteLine("Chose bottomLeft");
                pos = bottomLeft;
            }
            else if (textInBounds(topRight))
            {
                Console.WriteLine("Chose topRight");
                pos = topRight;
            }
            else if (textInBounds(bottomRight))
            {
                Console.WriteLine("Chose bottomRight");
                pos = bottomRight;
            }
            else
            {
                pos = Vector2.Zero;
            }

            if (pos.X + width > 1920)
            {
                Console.WriteLine("Right: " + (int)(pos.X + width));
            }

            for (int i = 0; i < textItems.Count; i++)
            {
                TextItem t = textItems.ToArray()[i];
                t.position = pos + new Vector2(0, i * t.rectangle.Height * 4);
                t.position.Y += (t.rectangle.Height * 4) / 2 - 3;
                t.position.X += 10;

            }

            backing = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(width, height, new Color(75,75,75)));
            backing.origin = Vector2.Zero;
            backing.position = pos;

            trigger = false;
        }

        bool textInBounds(Vector2 position)
        {
            Rectangle bounds = new Rectangle(0, 0, 1920, 1080);
            Vector2 topLeft = position;
            Vector2 bottomLeft = new Vector2(position.X, position.Y + height);
            Vector2 topRight = new Vector2(position.X + width, position.Y);
            Vector2 bottomRight = new Vector2(position.X + width, position.Y + height);

            return bounds.Contains(topLeft) && bounds.Contains(bottomLeft) &&
                bounds.Contains(topRight) && bounds.Contains(bottomRight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (trigger)
            {
                backing.Draw(spriteBatch);
                foreach (TextItem t in textItems)
                {
                    t.Draw(spriteBatch);
                }
            }
        }
    }
}
