using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pax_infinium
{
    interface IDrawable1
    {
        void Draw(SpriteBatch spriteBatch);
        int DrawOrder();

        void SetAlpha(float alpha);
    }
}
