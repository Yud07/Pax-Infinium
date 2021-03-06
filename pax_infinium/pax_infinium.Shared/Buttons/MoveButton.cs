﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.Buttons
{
    public class MoveButton : IButton
    {
        Polygon poly;
        Sprite sprite;
        TextItem text;
        DateTime clickTime;
        Sprite clickedFilter;
        bool trigger;
        Descriptor desc;

        public MoveButton(Vector2 pos)
        {
            int width = 300;
            int height = 50;
            sprite = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(width, height, Color.Gray));
            sprite.origin = Vector2.Zero;
            sprite.position = pos;
            poly = new Polygon();
            poly.Lines.Add(new PolyLine(pos, new Vector2(pos.X + width, pos.Y)));
            poly.Lines.Add(new PolyLine(pos, new Vector2(pos.X, pos.Y + height)));
            poly.Lines.Add(new PolyLine(new Vector2(pos.X + width, pos.Y), new Vector2(pos.X + width, pos.Y + height)));
            poly.Lines.Add(new PolyLine(new Vector2(pos.X, pos.Y + height), new Vector2(pos.X + width, pos.Y + height)));

            text = new TextItem(World.fontManager["Trajanus Roman 36"], "Move");
            text.color = Color.White;
            text.position = new Vector2(pos.X + width / 2, pos.Y + height / 2 + 3);

            clickTime = DateTime.MinValue;
            clickedFilter = new Sprite(Game1.world.textureConverter.GenRectangle(width, height, new Color(0, 0, 0, 75)));
            clickedFilter.origin = Vector2.Zero;
            clickedFilter.position = pos;

            desc = new Descriptor(poly, "(Keyboard M) Allows the character to move to a new position within their movement range." +
                " May be done once per turn before or after an action. When 'Undo' is displayed, it retracts that move if" +
                "they have not attacked or used their special.", sprite);

            trigger = false;
        }

        public void Click()
        {
            clickTime = DateTime.Now;
            trigger = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.world.level.grid.characters.list[0].team == 1)
            {
                sprite.visible = true;
                sprite.Draw(spriteBatch);
                text.Draw(spriteBatch);
                if (clickTime != DateTime.MinValue)
                {
                    if (DateTime.Now - clickTime < TimeSpan.FromSeconds(.5))
                    {
                        clickedFilter.Draw(spriteBatch);
                    }
                    else
                    {
                        clickTime = DateTime.MinValue;
                    }
                }
            }
            else
            {
                sprite.visible = false;
            }
        }

        public Descriptor GetDescriptor()
        {
            return desc;
        }

        public Polygon GetPoly()
        {
            return poly;
        }

        public bool GetTrigger()
        {
            return trigger;
        }

        public void ResetTrigger()
        {
            trigger = false;
        }

        public void SetText(string t)
        {
            int width = 300;
            int height = 50;
            Color temp = text.color;
            Vector2 pos = sprite.position;
            text = new TextItem(World.fontManager["Trajanus Roman 36"], t);
            text.color = temp;
            text.position = new Vector2(pos.X + width / 2, pos.Y + height / 2 + 3);
        }

        public void SetTextColor(Color c)
        {
            text.color = c;
        }
    }
}