using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace pax_infinium.Buttons
{
    public class UpButton : IButton
    {
        Polygon poly;
        Sprite sprite;
        TextItem arrowText;

        public UpButton(Vector2 pos)
        {
            int size = 40;
            sprite = new Sprite(Game1.world.textureConverter.GenBorderedRectangle(size, size, Color.Gray));
            sprite.origin = Vector2.Zero;
            sprite.position = pos;
            poly = new Polygon();
            poly.Lines.Add(new PolyLine(pos, new Vector2(pos.X + size, pos.Y)));
            poly.Lines.Add(new PolyLine(pos, new Vector2(pos.X, pos.Y + size)));
            poly.Lines.Add(new PolyLine(new Vector2(pos.X + size, pos.Y), new Vector2(pos.X + size, pos.Y + size)));
            poly.Lines.Add(new PolyLine(new Vector2(pos.X, pos.Y + size), new Vector2(pos.X + size, pos.Y + size)));

            arrowText = new TextItem(World.fontManager["Trajanus Roman 36"], "<-");
            arrowText.color = Color.Black;
            arrowText.position = new Vector2(pos.X + size/2 - 3, pos.Y + size/2);
            arrowText.rotation = 90;
        }

        public void Click()
        {
            Game1.world.level.grid.peel++;
            if (Game1.world.level.grid.peel > Game1.world.level.grid.height - 1)
            {
                Game1.world.level.grid.peel = Game1.world.level.grid.height - 1;
            }
            Game1.world.level.recalcPeelStatus();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            arrowText.Draw(spriteBatch);
        }

        public Polygon GetPoly()
        {
            return poly;
        }
    }
}
