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
        Texture2D westTex;
        Texture2D southTex;
        Texture2D topTex;
        GraphicsDeviceManager graphics;
        SpriteSheetInfo spriteSheetInfo;

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
                    tempColor = Color.Multiply(tempColor, .98f);
                }
                tempColor.A = color.A;
                mapcolors[s] = tempColor;
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
