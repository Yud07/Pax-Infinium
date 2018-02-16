using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pax_infinium.Buttons
{
    public interface IButton
    {
        Polygon GetPoly();

        void Click();

        void Draw(SpriteBatch spritebatch);

        bool GetTrigger();

        void ResetTrigger();

        void SetTextColor(Color c);
    }
}
