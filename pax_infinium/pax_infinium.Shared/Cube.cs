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
        public Sprite top;
        public Sprite north;
        public Sprite south;
        public Sprite east;
        public Sprite west;

        public Cube(Vector2 position, Texture2D topTex, Texture2D westTex, Texture2D southTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            top = new Sprite(topTex, graphics, spriteSheetInfo);
            top.position = position;
            top.position.X -= top.tex.Width / 2;// - 1;
            top.position.Y += top.tex.Height / 2;//  - 1;
            top.origin = new Vector2(topTex.Width / 2, topTex.Height / 2);
            top.scale = 1f;
            top.rotation = 180;
            //top.rotation = 25;

            west = new Sprite(westTex, graphics, spriteSheetInfo);
            west.position = position;
            west.position.X += top.tex.Width / 2;// - 1;
            west.position.Y += top.tex.Height / 2;// - 1;
            west.origin = new Vector2(westTex.Width / 2, westTex.Height / 2);
            west.scale = 1f;
            west.rotation = 180;
            //west.rotation = (float)(63.5 / 2.0);

            south = new Sprite(southTex, graphics, spriteSheetInfo);
            south.position = position;
            south.origin = new Vector2(southTex.Width / 2, southTex.Height / 2);
            south.scale = 1f;
        }

        public void Update(GameTime gameTime)
        {}

        public void Draw(SpriteBatch spriteBatch)
        {
            top.Draw(spriteBatch);
            west.Draw(spriteBatch);
            south.Draw(spriteBatch);
        }
    }
}
