using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public class Character
    {
        public Sprite top;
        public Vector2 position;
        public Vector3 gridPos;
        public Vector2 origin;
        public Texture2D topTex;
        GraphicsDeviceManager graphics;
        SpriteSheetInfo spriteSheetInfo;

        public Character(Vector2 origin, Vector3 gridPos, Texture2D topTex, GraphicsDeviceManager graphics, SpriteSheetInfo spriteSheetInfo)
        {
            this.topTex = topTex;
            this.origin = origin;
            this.gridPos = gridPos;
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * topTex.Width), (int)(gridPos.Y * topTex.Height * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * topTex.Height * .65F;
            this.graphics = graphics;
            this.spriteSheetInfo = spriteSheetInfo;

            top = new Sprite(topTex, graphics, spriteSheetInfo);
            top.position = position;
            top.origin = new Vector2(topTex.Width / 2, topTex.Height / 2);
            top.scale = 1f;
        }

        public void recalcPos()
        {
            this.position = origin + Game1.world.twoDToIso(new Point((int)(gridPos.X * topTex.Width), (int)(gridPos.Y * topTex.Height * .65f))).ToVector2();
            this.position.Y -= gridPos.Z * topTex.Height * .65F;

            top.position = position;

            //darken();
        }

        public void Update(GameTime gameTime)
        {
            //recalcPos();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            top.Draw(spriteBatch);
        }

    }
}
