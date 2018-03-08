using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium.Buttons
{
    public class SpecialButton : IButton
    {
        Polygon poly;
        Sprite sprite;
        TextItem text;
        DateTime clickTime;
        Sprite clickedFilter;
        bool trigger;
        Descriptor desc;

        public SpecialButton(Vector2 pos)
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

            text = new TextItem(World.fontManager["Trajanus Roman 36"], "Special");
            text.color = Color.White;
            text.position = new Vector2(pos.X + width / 2, pos.Y + height / 2 + 3);

            clickTime = DateTime.MinValue;
            clickedFilter = new Sprite(Game1.world.textureConverter.GenRectangle(width, height, new Color(0, 0, 0, 75)));
            clickedFilter.origin = Vector2.Zero;
            clickedFilter.position = pos;

            desc = new Descriptor(poly, "(Keyboard S) Conducts the character's special ability at a location. The Soldier's ability increases" +
                " the weapon resistance of the target. The Hunter' ability decreases the accuracy of the target. The" +
                " Mage's ability does health point damage to a cross of connected squares. The Healer's ability heals health points " +
                "in a cross of connected squares. The Thief's ability skips the target's turn and forces them to the end of the turn order.", sprite);

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

        public void SetText(string text)
        {
            throw new NotImplementedException();
        }

        public void SetTextColor(Color c)
        {
            text.color = c;
        }
    }
}