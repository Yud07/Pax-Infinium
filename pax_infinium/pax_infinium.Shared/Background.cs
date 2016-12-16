using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace pax_infinium
{
    public class Background
    {
        private Sprite sprite;
        private Viewport viewport;
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = new Vector2(value.X % sprite.tex.Width,
                    value.Y % sprite.tex.Height);
            }
        }

        public Background(Texture2D texture, Viewport viewport)
        {
            sprite = new Sprite(texture);
            sprite.origin = Vector2.Zero;
            this.viewport = viewport;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Math.Ceiling((float)viewport.Height / (float)sprite.tex.Height); y++)
            {
                for (int x = 0; x < Math.Ceiling((float)viewport.Width / (float)sprite.tex.Width); x++)
                {
                    spriteBatch.Draw(sprite.tex, new Vector2(Position.X + x * sprite.tex.Width, Position.Y + y * sprite.tex.Height), null, sprite.color * sprite.alpha, MathHelper.ToRadians(sprite.rotation), sprite.origin, sprite.scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}
