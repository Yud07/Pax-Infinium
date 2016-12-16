using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    public abstract class SpriteBase
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool visible;
        public Rectangle rectangle;
        public Color color;
        public Vector2 origin;
        public float alpha;
        public float rotation;
        public float scale;

        

        public SpriteBase()
        {
            SpriteInit();
        }

        private void SpriteInit()
        {
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            visible = true;
            rectangle = new Rectangle(0, 0, 0, 0);
            color = Color.White;
            origin = new Vector2(0, 0);
            alpha = 1.0f;
            rotation = 0.0f;
            scale = 1.0f;
        }

        public virtual void Update()
        {
            position += velocity;
        }

        public abstract void Draw(SpriteBatch spriteBatch);


    }
}
