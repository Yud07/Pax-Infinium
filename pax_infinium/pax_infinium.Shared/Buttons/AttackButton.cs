﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.Buttons
{
    public class AttackButton : IButton
    {
        Polygon poly;
        Sprite sprite;
        TextItem text;
        DateTime clickTime;
        Sprite clickedFilter;
        bool trigger;
        Descriptor desc;

        public AttackButton(Vector2 pos)
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

            text = new TextItem(World.fontManager["Trajanus Roman 36"], "Attack");
            text.color = Color.White;
            text.position = new Vector2(pos.X + width / 2, pos.Y + height / 2 + 3);

            clickTime = DateTime.MinValue;
            clickedFilter = new Sprite(Game1.world.textureConverter.GenRectangle(width, height, new Color(0, 0, 0, 75)));
            clickedFilter.origin = Vector2.Zero;
            clickedFilter.position = pos;

            desc = new Descriptor(poly, "(Keyboard A) Conducts a basic attack against a character within the character's attack range. Attacks damage the target's health points. Chance of hitting is lowest from the targets front, higher " +
                "from the sides, and highest from the back.", sprite);

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
            }
            else
            {
                sprite.visible = false;
            }
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

        public void SetTextColor(Color c)
        {
            text.color = c;
        }

        public Descriptor GetDescriptor()
        {
            return desc;
        }

        public void SetText(string text)
        {
            throw new NotImplementedException();
        }
    }
}